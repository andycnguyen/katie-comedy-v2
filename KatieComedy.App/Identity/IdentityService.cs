using System.Web;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using KatieComedy.App.Email;
using static KatieComedy.App.Constants;

namespace KatieComedy.App.Identity;

public class IdentityService(
    EmailSender emailSender,
    UserManager<IdentityUser> userManager,
    IUrlHelperFactory urlHelperFactory,
    IActionContextAccessor actionContextAccessor,
    IHttpContextAccessor httpContextAccessor)
{
    public async Task SendInvite(string email, string role, CancellationToken cancel)
    {
        var user = new IdentityUser
        {
            UserName = email,
            Email = email
        };

        var result = await userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception("Error creating user.");
        }

        result = await userManager.AddToRoleAsync(user, role);

        if (!result.Succeeded)
        {
            throw new Exception("Error assigning roles.");
        }

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext!);
        var callbackUrl = urlHelper.Page("/admin/users/register", null, null, httpContextAccessor.HttpContext?.Request.Scheme);

        if (string.IsNullOrEmpty(callbackUrl))
        {
            throw new Exception("Invalid callback Url.");
        }

        callbackUrl = QueryHelpers.AddQueryString(callbackUrl, new Dictionary<string, string?>
        {
            ["email"] = HttpUtility.UrlEncode(email),
            ["code"] = code
        });

        var htmlText =
        $"""
        You've been invited to create an account at katienguyen.com.
        <br/><br/> 
        Click <a href="{callbackUrl}">here</a> to register.
        """;

        await emailSender.SendEmailAsync(new EmailRequest
        {
            ToAddress = email,
            ToName = email.Split('@').First(),
            Subject = "Register at katienguyen.com",
            HtmlText = htmlText
        }, cancel);
    }

    public async Task Register(string email, string password, string code, CancellationToken cancel)
    {
        var user = await userManager.FindByEmailAsync(email) ?? throw new Exception("User not found.");

        IdentityResult result;

        if (!user.EmailConfirmed)
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            result = await userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to confirm user email: {string.Join(", ", result.Errors.Select(x => x.Description))}");
            }
        }

        if (await userManager.HasPasswordAsync(user))
        {
            await userManager.RemovePasswordAsync(user);
        }

        result = await userManager.AddPasswordAsync(user, password);

        if (!result.Succeeded)
        {
            throw new Exception($"Failed to set password: {string.Join(", ", result.Errors.Select(x => x.Description))}");
        }
    }

    public async Task DeleteUser(string id)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user is null)
        {
            throw new Exception("User not found.");
        }

        if (await userManager.IsInRoleAsync(user, Roles.Owner))
        {
            throw new Exception("Cannot delete owner.");
        }

        await userManager.DeleteAsync(user);
    }
}
