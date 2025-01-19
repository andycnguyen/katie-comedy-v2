using KatieComedy.App.Identity;

namespace KatieComedy.Web.Pages.Admin.Users;

public class InviteModel(IdentityService service) : BasePageModel
{
    [BindProperty, Required, EmailAddress]
    public string? Email { get; set; }

    [BindProperty, Required]
    public string? Role { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancel)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await service.SendInvite(Email!, Role!, cancel);

        Toast(ToastLevel.Success, "Invite sent.");
        return RedirectToPage("Index");
    }
}
