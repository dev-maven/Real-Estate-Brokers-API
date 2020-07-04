using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Urban.ng.Models;

namespace Urban.ng.Data
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)

        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                //var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                //var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role>
                {
                    new Role{Name = "Customer"},
                    new Role{Name = "Owner"},
                    new Role{Name = "Individual Agent"},
                    new Role{Name = "Cooperate Agent"},
                    new Role{Name = "Admin"}
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }

                var adminUser = new User
                {
                    UserName = "admin@urban.ng"
                };

                IdentityResult result = _userManager.CreateAsync(adminUser, "password").Result;

                if (result.Succeeded)
                {
                    var admin =_userManager.FindByNameAsync("admin@urban.ng").Result;
                    _userManager.AddToRoleAsync(admin, "Admin").Wait();
                }
            }


        }
    }
}

