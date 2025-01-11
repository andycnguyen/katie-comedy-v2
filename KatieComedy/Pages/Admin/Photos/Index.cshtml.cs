using KatieComedy.App.Photos;

namespace KatieComedy.Web.Pages.Admin.Photos;

public class IndexModel(PhotoService service) : BasePageModel
{
    public IReadOnlyList<Photo> Photos { get; set; } = [];

    public async Task OnGetAsync(CancellationToken cancel)
    {
        Photos = await service.Get(cancel);
    }
}
