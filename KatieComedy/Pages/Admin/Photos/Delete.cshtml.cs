using KatieComedy.App.Photos;

namespace KatieComedy.Web.Pages.Admin.Photos;

public class DeleteModel(PhotoService service) : PageModel
{
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(int id)
    {
        await service.Delete(id);
        return RedirectToPage("Index");
    }
}
