using KatieComedy.App.Media;

namespace KatieComedy.Web.Pages.Admin.Media;

public class DeleteModel(MediaService service) : BasePageModel
{
    public async Task<IActionResult> OnPost(int id)
    {
        await service.Delete(id);
        Toast(ToastLevel.Success, "Media deleted");
        return RedirectToPage("Index");
    }
}
