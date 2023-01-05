using Application.Enums;
using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Seeds;

public static class DefaultBasicUser
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var defaultUser = new ApplicationUser
        {
            UserName = "userBasic",
            Email = "useradmin@email.com",
            Name = "Pedro",
            LastName = "Vasquez",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user is null)
            {
                await userManager.CreateAsync(defaultUser, "123Pa$word");
                await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
            }
        }
    }
}