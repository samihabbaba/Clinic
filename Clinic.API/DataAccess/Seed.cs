using System.Collections.Generic;
using System.Linq;
using Clinic.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Clinic.API.DataAccess
{
    public class Seed
    {
        public static void SeedUsers(UserManager<SystemUser> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<SystemUser>>(userData);

                // create some roles

                var roles = new List<Role>
                {
                    new Role{Name = "Patient"},
                    new Role{Name = "Doctor"},
                };

                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }



                foreach (var user in users)
                {
                    userManager.CreateAsync(user, "password").Wait();
                    userManager.AddToRoleAsync(user, "Patient");
                }

                //  create doctor user

                var doctorUser = new SystemUser
                {
                    UserName = "Doctor"
                };

                var result = userManager.CreateAsync(doctorUser, "password").Result;

                if (result.Succeeded)
                {
                    var doctor = userManager.FindByNameAsync("Doctor").Result;
                    userManager.AddToRolesAsync(doctor, new[] {"Doctor"});
                }
            }
        }

    }
}