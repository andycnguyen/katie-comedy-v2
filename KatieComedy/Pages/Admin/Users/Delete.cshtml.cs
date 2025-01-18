using KatieComedy.App.Identity;

namespace KatieComedy.Web.Pages.Admin.Users;

public class DeleteModel(IdentityService service) : BasePageModel
{
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(string id)
    {
        await service.DeleteUser(id);
        Toast(ToastLevel.Success, "User deleted");
        return RedirectToPage("Index");
    }
}
