using KatieComedy.App.Media;

namespace KatieComedy.Web.Pages.Admin.Media;

public class DeleteModel(MediaService service) : PageModel
{
    public async Task<IActionResult> OnPost(int id)
    {
        await service.Delete(id);
        return RedirectToPage("Index");
    }
}
