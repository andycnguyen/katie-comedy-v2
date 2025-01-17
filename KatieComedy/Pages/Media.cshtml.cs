using KatieComedy.App.Media;

namespace KatieComedy.Web.Pages;

public class MediaModel(MediaService service) : PageModel
{
    public IReadOnlyList<Media> Media { get; set; } = [];

    public async Task OnGet(CancellationToken cancel)
    {
        Media = await service.Get(cancel);
    }
}
