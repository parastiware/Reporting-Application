using Microsoft.AspNetCore.Identity;
using ReportingApplication.Data.Entities;

namespace ReportingApplication.Data.Migrations
{
    public static class Seed
    {
        public static async Task CreateRoles(IServiceCollection services, IConfiguration Configuration)
        {
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
          

            //adding customs roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] roleNames = { "SuperAdmin", "Admin", "Manager", "Finance", "Employee" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                //creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //creating a super user who could maintain the web app
            var poweruser = new IdentityUser
            {
                UserName = Configuration.GetSection("AppSettings")["UserEmail"],
                Email = Configuration.GetSection("AppSettings")["UserEmail"]
            };

            string userPassword = Configuration.GetSection("AppSettings")["UserPassword"]??"Admin";
            var user = await UserManager.FindByEmailAsync(Configuration.GetSection("AppSettings")["UserEmail"] ?? "Admin");

            if (user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the "SuperAdmin" role 
                    await UserManager.AddToRoleAsync(poweruser, "SuperAdmin");

                }
            }
            }
        }
    }
}
