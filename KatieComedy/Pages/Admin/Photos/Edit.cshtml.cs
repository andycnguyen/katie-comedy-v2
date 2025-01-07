using KatieComedy.App.Photos;

namespace KatieComedy.Web.Pages.Admin.Photos;

public class EditModel(PhotoService service) : PageModel
{
    public async Task OnGetAsync(int id)
    {
        var photo = await service.Get(id);
    }
}
