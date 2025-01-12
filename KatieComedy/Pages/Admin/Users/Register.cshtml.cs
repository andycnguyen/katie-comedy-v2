using System.Web;
using Microsoft.AspNetCore.Identity;
using KatieComedy.App.Identity;

namespace KatieComedy.Web.Pages.Admin.Users;

public class RegisterModel(
    UserManager<IdentityUser> userManager,
    IdentityService service,
    SignInManager<IdentityUser> signInManager) : BasePageModel
{
    [FromQuery(Name = "email")]
    public string UrlEncodedEmail { get; set; }

    [Display(Name = "Email")]
    public string DecodedEmail => HttpUtility.UrlDecode(UrlEncodedEmail);

    [FromQuery(Name = "code")]
    public string Code { get; set; }

    [Required, MinLength(6), MaxLength(64), DataType(DataType.Password)]
    public string Password { get; set; }

    public async Task<IActionResult> OnGet()
    {
        if (signInManager.IsSignedIn(User))
        {
            await signInManager.SignOutAsync();

            // must redirect to update identity
            return RedirectToPage(null, new
            {
                code = Code,
                email = UrlEncodedEmail
            });
        }

        var idUser = await userManager.FindByEmailAsync(DecodedEmail);

        if (idUser is null)
        {
            return BadRequest();
        }

        if (idUser.EmailConfirmed)
        {
            Toast(ToastLevel.Primary, "Please login to continue.");
            return RedirectToPage("/Identity/Account/Login");
        }

        return Page();
    }

    public async Task<IActionResult> OnPost(CancellationToken cancel)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await service.Register(DecodedEmail, Password, Code, cancel);
        Toast(ToastLevel.Success, "Registration successful. Please login.");
        return RedirectToPage("/Account/Login");
    }
}
