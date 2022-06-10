using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Plugins;
using Operativka.Areas.Identity.Models;
using System;

namespace Operativka.Areas.Identity.Data
{
    public class ContextSeeder
    {
        private readonly IConfiguration _config;
        public ContextSeeder(IConfiguration configuration)
        {
            _config = configuration;
        }
        public async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            foreach (var role in Enum.GetValues<Enums.Roles>())
            {
                await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }

            //await roleManager.CreateAsync(new IdentityRole(Enums.Roles.SuperAdmin.ToString()));
            //await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            //await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Moderator.ToString()));
            //await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Basic.ToString()));
        }

        public async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            
            //var pwd = builder.Configuration["Priviliegies:SuperAdmins:SuperAdmin:Pwd"];
            // Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = _config["Identity:Accounts:SuperAdmin:Credentials:UserName"],
                Email = "acuta.pugione@gmail.com",
                FirstName = "Дмитро",
                LastName = "Дементєєв",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, _config["Identity:Accounts:SuperAdmin:Credentials:Pwd"]);

                    foreach (var role in Enum.GetValues<Enums.Roles>())
                    {
                        await userManager.AddToRoleAsync(defaultUser, role.ToString());
                    }
                    //await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Basic.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Moderator.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, Enums.Roles.SuperAdmin.ToString());
                }

            }
        }

        public async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            //var pwd = builder.Configuration["Priviliegies:SuperAdmins:SuperAdmin:Pwd"];
            // Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = _config["Identity:Accounts:Admin:Credentials:UserName"],
                Email = "Secretary@gaz.kherson.ua",
                FirstName = "Admin",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, _config["Identity:Accounts:Admin:Credentials:Pwd"]);

                    foreach (var role in Enum.GetValues<Enums.Roles>())
                    {
                        if (role!=Enums.Roles.SuperAdmin)
                        {
                            await userManager.AddToRoleAsync(defaultUser, role.ToString());
                        }
                    }
                    //await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Basic.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Moderator.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, Enums.Roles.SuperAdmin.ToString());
                }

            }
        }
    }
}
