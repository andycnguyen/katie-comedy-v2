using KatieComedy.App.Photos;

namespace KatieComedy.Web.Pages.Admin.Photos;

public class DeleteModel(PhotoService service) : BasePageModel
{
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(int id)
    {
        await service.Delete(id);
        Toast(ToastLevel.Success, "Photo deleted.");
        return RedirectToPage("Index");
    }
}
