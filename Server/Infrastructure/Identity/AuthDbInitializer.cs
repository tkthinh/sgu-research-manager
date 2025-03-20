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

         // Seed the admin user (username: "admin", role: Admin)
         if (await userManager.FindByNameAsync("admin") == null)
         {
            var adminUser = new ApplicationUser
            {
               UserName = "admin",
               Email = "admin@example.com",
               EmailConfirmed = true,
               IsApproved = true
            };

            var adminDto = new UserDto
            {
               UserName = "admin",
               Email = "admin@sgu.edu.vn",
               AcademicTitle = "TS",
               FullName = "Ạc Min",
               OfficerRank = "GiangVien",
               IdentityId = adminUser.Id,
               DepartmentId = Guid.Parse("334d3f98-43b1-4dbb-9809-eff2c70f0441"),
               FieldId = Guid.Parse("222af233-e26e-4e98-a509-4bafa2657512"),
            };

            // Use a secure password in production
            var accountResult = await userManager.CreateAsync(adminUser, "123456");
            var infoResult = await userService.CreateAsync(adminDto);
            if (accountResult.Succeeded && !String.IsNullOrEmpty(infoResult.Id.ToString()))
            {
               await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {
               foreach (var error in accountResult.Errors)
               {
                  Console.WriteLine($"Admin user error: {error.Description}");
               }
            }
         }

         // Seed the normal user (username: "user", role: User)
         if (await userManager.FindByNameAsync("user") == null)
         {
            var normalUser = new ApplicationUser
            {
               UserName = "user",
               Email = "user@example.com",
               EmailConfirmed = true,
               IsApproved = true
            };

            var userDto = new UserDto
            {
               UserName = "user",
               Email = "user@sgu.edu.vn",
               AcademicTitle = "ThS", 
               FullName = "U Sờ",
               OfficerRank = "GiangVien",   
               IdentityId = normalUser.Id,
               DepartmentId = Guid.Parse("334d3f98-43b1-4dbb-9809-eff2c70f0441"),
               FieldId = Guid.Parse("222af233-e26e-4e98-a509-4bafa2657512"),
            };

            var accountResult = await userManager.CreateAsync(normalUser, "123456");
            var infoResult = await userService.CreateAsync(userDto);
            if (accountResult.Succeeded && !String.IsNullOrEmpty(infoResult.Id.ToString()))
            {
               await userManager.AddToRoleAsync(normalUser, "User");
            }
            else
            {
               foreach (var error in accountResult.Errors)
               {
                  Console.WriteLine($"User error: {error.Description}");
               }
            }
         }

         // Seed the manager user (username: "manager", role: Manager)
         if (await userManager.FindByNameAsync("manager") == null)
         {
            var managerUser = new ApplicationUser
            {
               UserName = "manager",
               Email = "manager@example.com",
               EmailConfirmed = true,
               IsApproved = true
            };

            var managerDto = new UserDto
            {
               UserName = "manager",
               Email = "manager@sgu.edu.vn",
               AcademicTitle = "CN", 
               FullName = "Quản Lý",
               OfficerRank = "ChuyenVien",   
               IdentityId = managerUser.Id,
               DepartmentId = Guid.Parse("334d3f98-43b1-4dbb-9809-eff2c70f0441"),
               FieldId = Guid.Parse("222af233-e26e-4e98-a509-4bafa2657512"),
            };

            var accountResult = await userManager.CreateAsync(managerUser, "123456");
            var infoResult = await userService.CreateAsync(managerDto);
            if (accountResult.Succeeded && !String.IsNullOrEmpty(infoResult.Id.ToString()))
            {
               await userManager.AddToRoleAsync(managerUser, "Manager");
            }
            else
            {
               foreach (var error in accountResult.Errors)
               {
                  Console.WriteLine($"Manager user error: {error.Description}");
               }
            }
         }
      }
   }
}
