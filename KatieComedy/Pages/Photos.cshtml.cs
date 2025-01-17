using KatieComedy.App.Photos;

namespace KatieComedy.Web.Pages;

public class PhotosModel(PhotoService service) : PageModel
{
    public IReadOnlyList<Photo> Photos { get; set; } = [];

    public async Task OnGet(CancellationToken cancel)
    {
        Photos = await service.Get(cancel);
    }
}
