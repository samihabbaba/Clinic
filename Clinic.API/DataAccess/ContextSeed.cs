using System.Linq;
using System.Threading.Tasks;
using Clinic.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Clinic.API.DataAccess
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<SystemUser> userManager, RoleManager<IdentityRole> roleManager)
        
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Models.Roles.Patient.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Models.Roles.Doctor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Models.Roles.Admin.ToString()));
        }


        public static async Task SeedAdminAsync(UserManager<SystemUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new SystemUser 
            { 
                UserName = "admin", 
                Email = "admin@gmail.com",
                Name = "Dev",
                Surname = "Sammy",
                EmailConfirmed = true, 
                PhoneNumberConfirmed = true 
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if(user==null)
                {
                    await userManager.CreateAsync(defaultUser, "Password123!");
                    await userManager.AddToRoleAsync(defaultUser, Models.Roles.Admin.ToString());
                }
                    
            }
        }
    }
}