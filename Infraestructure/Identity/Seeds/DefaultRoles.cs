using Application.Enums;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Seeds;

public static class DefaultRoles
{
    public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        var userRoleExist = await roleManager.RoleExistsAsync(Roles.User.ToString());
        if (!userRoleExist) await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

        var adminRoleExist = await roleManager.RoleExistsAsync(Roles.Admin.ToString());
        if (!adminRoleExist) await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
    }
}