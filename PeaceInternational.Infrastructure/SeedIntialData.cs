using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace PeaceInternational.Infrastructure
{
    public class SeedIntialData
    {      

        public static async Task Initialize(IServiceProvider serviceProvider)           
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();


            string[] Roles = { "ADMIN", "USER" };

            foreach (string roleName in Roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            if (await userManager.FindByNameAsync("admin") == null)
            {
                var user = new IdentityUser()
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    PhoneNumber = "4413262"                  
                };

                var res = await userManager.CreateAsync(user, "Admin@123");
                await userManager.SetLockoutEnabledAsync(user, false);
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "ADMIN");
                }
            }            
        }
    }
}
