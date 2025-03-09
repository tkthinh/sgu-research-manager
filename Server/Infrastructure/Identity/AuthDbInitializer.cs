using Domain.Enums;
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
         var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
         var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

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
            var adminUser = new IdentityUser
            {
               UserName = "admin",
               Email = "admin@example.com",
               EmailConfirmed = true
            };

            // Use a secure password in production
            var result = await userManager.CreateAsync(adminUser, "123456");
            if (result.Succeeded)
            {
               await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {
               foreach (var error in result.Errors)
               {
                  Console.WriteLine($"Admin user error: {error.Description}");
               }
            }
         }

         // Seed the manager user (username: "123123", role: Manager)
         if (await userManager.FindByNameAsync("123123") == null)
         {
            var managerUser = new IdentityUser
            {
               UserName = "123123",
               Email = "manager@example.com",
               EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(managerUser, "123456");
            if (result.Succeeded)
            {
               await userManager.AddToRoleAsync(managerUser, "Manager");
            }
            else
            {
               foreach (var error in result.Errors)
               {
                  Console.WriteLine($"Manager user error: {error.Description}");
               }
            }
         }

         // Seed the normal user (username: "234234", role: User)
         if (await userManager.FindByNameAsync("234234") == null)
         {
            var normalUser = new IdentityUser
            {
               UserName = "234234",
               Email = "user@example.com",
               EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(normalUser, "123456");
            if (result.Succeeded)
            {
               await userManager.AddToRoleAsync(normalUser, "User");
            }
            else
            {
               foreach (var error in result.Errors)
               {
                  Console.WriteLine($"Normal user error: {error.Description}");
               }
            }
         }
      }
   }
}
