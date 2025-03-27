using System.Globalization;
using System.Transactions;
using Application.Departments;
using Application.Shared.Response;
using Application.Users;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;

namespace Infrastructure.Identity.Services
{
    public class UserImportService : IUserImportService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;
        private readonly IDepartmentService departmentService;

        public UserImportService(
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            IDepartmentService departmentService
            )
        {
            this.userManager = userManager;
            this.userService = userService;
            this.departmentService = departmentService;
        }

        public async Task<ApiResponse<object>> ImportUsersAsync(Stream excelStream)
        {
            try
            {
                // EPPlus requires setting a license context
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using var package = new ExcelPackage(excelStream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    return new ApiResponse<object>(false, "Không tìm thấy worksheet nào trong file Excel.");

                int rowCount = worksheet.Dimension.Rows;
                int importedCount = 0;
                int skippedCount = 0;

                // Loop through rows (assuming first row is header)
                for (int row = 2; row <= rowCount; row++)
                {
                    // Read cells:
                    var username = worksheet.Cells[row, 1].GetValue<string>()?.Trim();
                    var fullName = worksheet.Cells[row, 2].GetValue<string>()?.Trim();

                    var dobString = worksheet.Cells[row, 3].GetValue<string>()?.Trim();
                    DateTime dobValue;
                    if (!DateTime.TryParseExact(dobString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dobValue))
                    {
                        // If parsing fails, skip this row
                        skippedCount++;
                        continue;
                    }

                    var departmentName = worksheet.Cells[row, 4].GetValue<string>()?.Trim();
                    var roleFromExcel = worksheet.Cells[row, 5].GetValue<string>()?.Trim();

                    // Validate required fields
                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(fullName))
                    {
                        skippedCount++;
                        continue;
                    }

                    // Check if the identity user already exists
                    var existingIdentityUser = await userManager.FindByNameAsync(username);
                    if (existingIdentityUser != null)
                    {
                        skippedCount++;
                        continue;
                    }

                    // Create default password from DOB in ddMMyyyy format
                    string defaultPassword = dobValue.ToString("ddMMyyyy", CultureInfo.InvariantCulture);

                    string identityRole = roleFromExcel?.ToLower() switch
                    {
                        "quản trị" => "Admin",
                        "quản lý" => "Manager",
                        _ => "User", // default to user if "người dùng" or any other value
                    };

                    using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        // Create the Identity user
                        var newIdentityUser = new ApplicationUser
                        {
                            UserName = username,
                            IsApproved = true
                        };
                        var identityResult = await userManager.CreateAsync(newIdentityUser, defaultPassword);
                        if (!identityResult.Succeeded)
                        {
                            skippedCount++;
                            continue;
                        }

                        // Map Excel role to Identity roles


                        var roleResult = await userManager.AddToRoleAsync(newIdentityUser, identityRole);
                        if (!roleResult.Succeeded)
                        {
                            skippedCount++;
                            continue;
                        }

                        // Create the domain user.
                        // Assuming that you have a method in IUserService to get DepartmentId by department name.
                        var department = await departmentService.GetDepartmentByNameAsync(departmentName!);
                        Guid departmentId = department?.Id ?? Guid.Empty;

                        // Map to your domain user DTO. Adjust properties as needed.
                        var userDto = new UserDto
                        {
                            FullName = fullName,
                            UserName = username,
                            IdentityId = newIdentityUser.Id,
                            DepartmentId = departmentId
                        };

                        var domainUser = await userService.CreateAsync(userDto);
                        if (domainUser != null)
                        {
                            importedCount++;
                        }
                        else
                        {
                            skippedCount++;
                        }
                    }
                }

                return new ApiResponse<object>(true, $"Nhập thành công: {importedCount}, Bỏ qua: {skippedCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResponse<object>(false, "Lỗi khi nhập dữ liệu từ Excel");
            }
        }
    }
}
