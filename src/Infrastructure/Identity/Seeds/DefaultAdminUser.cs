using Application.Enums;
using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Seeds;

public static class DefaultAdminUser
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var defaultUser = new ApplicationUser
        {
            UserName = "userAdmin",
            Email = "useradmin@email.com",
            Name = "Nicolas",
            LastName = "Cruz",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if(userManager.Users.All(u=>u.Id!=defaultUser.Id)){
            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if(user is null){
                await userManager.CreateAsync(defaultUser, "123Pa$word");
                await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
            }
        }
    }
}