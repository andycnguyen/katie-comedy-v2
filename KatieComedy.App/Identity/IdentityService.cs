using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KatieComedy.App.Email;

namespace KatieComedy.App.Identity;

public class IdentityService(
    EmailSender emailSender,
    UserManager<IdentityUser> userManager,
    IUrlHelper urlHelper,
    IHttpContextAccessor httpContextAccessor)
{
    public async Task SendInvite(string email, string role)
    {
        var callbackUrl = urlHelper.Page("/admin/users/register", null, null, httpContextAccessor.HttpContext?.Request.Scheme);

        if (string.IsNullOrEmpty(callbackUrl))
        {
            throw new Exception("Invalid callback Url.");
        }

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
        }, default);
    }
}
