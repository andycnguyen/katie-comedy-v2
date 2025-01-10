using KatieComedy.App.Media;

namespace KatieComedy.Web.Pages.Admin.Media;

public class IndexModel(MediaService service) : PageModel
{
    public IReadOnlyList<App.Media.Media> Media { get; set; }

    public async Task OnGet(CancellationToken cancel)
    {
        Media = await service.Get(cancel);
    }
}
