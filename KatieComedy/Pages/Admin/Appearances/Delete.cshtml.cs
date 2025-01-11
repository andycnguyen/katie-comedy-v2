using KatieComedy.App.Appearances;

namespace KatieComedy.Web.Pages.Admin.Appearances;

public class DeleteModel(AppearanceService service) : BasePageModel
{
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await service.Delete(id);
        Toast(ToastLevel.Success, "Appearance deleted");
        return RedirectToPage("Index");
    }
}
