using Application.Users;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public static class AuthDbInitializer
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        // Create a scope to retrieve scoped services
        using (var scope = serviceProvider.CreateScope())
        {
            // Resolve UserManager and RoleManager
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            // Define the roles to be created
            string[] roles = { "Admin", "Manager", "User" };

            // Create roles if they do not exist
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed 3 admin users
            for (int i = 1; i <= 3; i++)
            {
                string username = $"admin{i}";
                if (await userManager.FindByNameAsync(username) == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = username,
                        Email = $"{username}@sgu.edu.vn",
                        EmailConfirmed = true,
                        IsApproved = true
                    };

                    var adminDto = new UserDto
                    {
                        UserName = username,
                        Email = $"{username}@sgu.edu.vn",
                        PhoneNumber = "0123456789",
                        Specialization = "Chuyên ngành Test",
                        AcademicTitle = "TS",
                        FullName = $"Admin {i}",
                        OfficerRank = "GiangVien",
                        IdentityId = adminUser.Id,
                        DepartmentId = Guid.Parse("334d3f98-43b1-4dbb-9809-eff2c70f0441"),
                        FieldId = Guid.Parse("222af233-e26e-4e98-a509-4bafa2657512"),
                    };

                    // Use a secure password in production
                    var accountResult = await userManager.CreateAsync(adminUser, "123456");
                    var infoResult = await userService.CreateAsync(adminDto);
                    if (accountResult.Succeeded && !string.IsNullOrEmpty(infoResult.Id.ToString()))
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        foreach (var error in accountResult.Errors)
                        {
                            Console.WriteLine($"Admin user {username} error: {error.Description}");
                        }
                    }
                }
            }

            // Seed 6 manager users
            for (int i = 1; i <= 6; i++)
            {
                string username = $"manager{i}";
                if (await userManager.FindByNameAsync(username) == null)
                {
                    var managerUser = new ApplicationUser
                    {
                        UserName = username,
                        Email = $"{username}@sgu.edu.vn",
                        EmailConfirmed = true,
                        IsApproved = true
                    };

                    var managerDto = new UserDto
                    {
                        UserName = username,
                        Email = $"{username}@sgu.edu.vn",
                        PhoneNumber = "0123456789",
                        Specialization = "Chuyên ngành Test",
                        AcademicTitle = "CN",
                        FullName = $"Manager {i}",
                        OfficerRank = "ChuyenVien",
                        IdentityId = managerUser.Id,
                        DepartmentId = Guid.Parse("334d3f98-43b1-4dbb-9809-eff2c70f0441"),
                        FieldId = Guid.Parse("222af233-e26e-4e98-a509-4bafa2657512"),
                    };

                    var accountResult = await userManager.CreateAsync(managerUser, "123456");
                    var infoResult = await userService.CreateAsync(managerDto);
                    if (accountResult.Succeeded && !string.IsNullOrEmpty(infoResult.Id.ToString()))
                    {
                        await userManager.AddToRoleAsync(managerUser, "Manager");
                    }
                    else
                    {
                        foreach (var error in accountResult.Errors)
                        {
                            Console.WriteLine($"Manager user {username} error: {error.Description}");
                        }
                    }
                }
            }
        }
    }
}
