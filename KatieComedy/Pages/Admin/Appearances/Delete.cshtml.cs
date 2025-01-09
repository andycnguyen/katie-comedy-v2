using KatieComedy.App.Appearances;

namespace KatieComedy.Web.Pages.Admin.Appearances;

public class DeleteModel(AppearanceService service) : PageModel
{
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await service.Delete(id);
        return RedirectToPage("Index");
    }
}
