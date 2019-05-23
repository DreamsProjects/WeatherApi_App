using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherGroup2.Models;

namespace WeatherGroup2.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {

        public virtual DbSet<Favorite> Favorites { get; set; }
       
        // Constructor
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options)
        {

        }

        public static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            // 1. Retrieve UserManager and roleManager objects via IServiceProvider object
            UserManager<AppUser> userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 2. retrieve data for admin account from appsettings.json file
            string username = configuration["Data:AdminUser:UserName"];
            string email = configuration["Data:AdminUser:Email"];
            string password = configuration["Data:AdminUser:Password"];
            string role = configuration["Data:AdminUser:Role"];


            // 3. Does the admin user just retrieved from appsettings.json is in DB?
            if (await userManager.FindByNameAsync(username) == null)
            {
                // 4. Does the role of the admin user just retrieved from appsettings.json is in DB?
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    // 5. Create the role
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                // 6. Create the user model 
                AppUser user = new AppUser
                {
                    UserName = username,
                    Email = email
                };

                // 7. Save the user in DB (Update the AspNetUsers table)
                IdentityResult result = await userManager.CreateAsync(user, password);

                // 8. Assign the Role to the user and save in DB (Update the AspNetRoles table
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }

    }
}
