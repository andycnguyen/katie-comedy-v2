using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using static KatieComedy.App.Constants;

namespace KatieComedy.App.Identity;

public class IdentityInitializer(
    UserManager<IdentityUser> userManager,
    ApplicationDbContext dbContext,
    IOptions<IdentityOptions> options)
{
    private readonly IdentityOptions _options = options.Value;

    public async Task Initialize()
    {
        var roleStore = new RoleStore<IdentityRole, ApplicationDbContext>(dbContext);
        var addRoles = new[] { Roles.Owner, Roles.Admin }.Except(roleStore.Roles.Select(x => x.Name));

        foreach (var role in addRoles)
        {
            var result = await roleStore.CreateAsync(new IdentityRole
            {
                Name = role,
                NormalizedName = role?.ToUpper()
            });
        }

        var admins = await userManager.GetUsersInRoleAsync(Roles.Owner);

        if (admins.Any())
        {
            return;
        }

        var defaultAdmin = new IdentityUser
        {
            UserName = options.Value.DefaultAdminEmail,
            Email = options.Value.DefaultAdminEmail,
            EmailConfirmed = true
        };

        var idUserResult = await userManager.CreateAsync(defaultAdmin, options.Value.DefaultAdminPassword);

        if (!idUserResult.Succeeded)
        {
            throw new Exception("Error initializing default admin");
        }

        var idUser = await userManager.FindByEmailAsync(_options.DefaultAdminEmail);
        idUserResult = await userManager.AddToRoleAsync(idUser!, Roles.Owner);

        if (!idUserResult.Succeeded)
        {
            throw new Exception("Error initializing default admin");
        }
    }
}
