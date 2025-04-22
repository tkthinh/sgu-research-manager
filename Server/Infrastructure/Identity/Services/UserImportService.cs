using System.Globalization;
using System.Transactions;
using Application.Departments;
using Application.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace Infrastructure.Identity.Services
{
    public class UserImportService : IUserImportService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;
        private readonly IDepartmentService departmentService;
        private readonly ILogger<UserImportService> logger;

        public UserImportService(
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            IDepartmentService departmentService,
            ILogger<UserImportService> logger
            )
        {
            this.userManager = userManager;
            this.userService = userService;
            this.departmentService = departmentService;
            this.logger = logger;
        }

        public async Task<UserImportResult> ImportUsersAsync(Stream excelStream)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                using var package = new ExcelPackage(excelStream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    throw new Exception("Không tìm thấy worksheet nào trong file Excel.");

                int rowCount = worksheet.Dimension.Rows;
                int importedCount = 0;
                int skippedCount = 0;

                var userstoImport = new List<(ApplicationUser userAuth, UserDto userInfo, string defaultPassword)>();

                // Loop through rows (assuming first row is header)
                for (int row = 2; row <= rowCount; row++)
                {
                    bool isEmptyRow = Enumerable.Range(1, 5)
                                        .All(col => string.IsNullOrWhiteSpace(
                                            worksheet.Cells[row, col]
                                            .GetValue<string>()));

                    if (isEmptyRow)
                        continue;

                    // Read cells:
                    var username = worksheet.Cells[row, 1].GetValue<string>()?.Trim();
                    var fullName = worksheet.Cells[row, 2].GetValue<string>()?.Trim();

                    var dobString = worksheet.Cells[row, 3].GetValue<string>()?.Trim();
                    DateTime dobValue;
                    if (!DateTime.TryParseExact(dobString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dobValue))
                    {
                        throw new ValidationException($"Ngày sinh không hợp lệ ở hàng {row}");
                    }

                    var departmentName = worksheet.Cells[row, 4].GetValue<string>()?.Trim();
                    var roleFromExcel = worksheet.Cells[row, 5].GetValue<string>()?.Trim();

                    // Validate required fields
                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(fullName))
                    {
                        throw new Exception($"Tên người dùng hoặc họ tên không hợp lệ ở hàng {row}");
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

                    // Create the Identity user
                    var newIdentityUser = new ApplicationUser
                    {
                        UserName = username,
                        IsApproved = true
                    };

                    // Create the domain user.
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

                    // Store for batch processing
                    userstoImport.Add((newIdentityUser, userDto, defaultPassword));
                }

                foreach (var (userAuth, userInfo, defaultPassword) in userstoImport)
                {
                    // Create Identity user
                    var identityResult = await userManager.CreateAsync(userAuth, defaultPassword);
                    if (!identityResult.Succeeded)
                    {
                        throw new InvalidOperationException("Tạo identity user thất bại");
                    }

                    // Assign role
                    var roleResult = await userManager.AddToRoleAsync(userAuth, "User");
                    if (!roleResult.Succeeded)
                    {
                        throw new InvalidOperationException("Phân quyền user thất bại");
                    }

                    // Set Identity ID for domain user
                    userInfo.IdentityId = userAuth.Id;

                    // Create domain user
                    var domainUser = await userService.CreateAsync(userInfo);
                    if (domainUser == null)
                    {
                        throw new InvalidOperationException("Tạo người dùng thất bại");
                    }

                    importedCount++;
                }
                transactionScope.Complete();

                return new UserImportResult
                {
                    ImportedCount = importedCount,
                    SkippedCount = skippedCount
                };
            }
            catch (ValidationException vex)
            {
                logger.LogError($"Validation Error: {vex.Message}");
                throw new Exception(vex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error importing users from Excel");
                throw new Exception("Lỗi khi nhập dữ liệu từ Excel");
            }
        }

        public class ValidationException : Exception
        {
            public ValidationException(string message) : base(message) { }
        }
    }
}
