using KatieComedy.App.Appearances;

namespace KatieComedy.Web.Pages.Admin.Appearances;

public class IndexModel(AppearanceService service) : PageModel
{
    public IReadOnlyList<Appearance> Appearances { get; set; } = [];

    public async Task OnGetAsync(CancellationToken cancel)
    {
        Appearances = await service.Get(cancel);
    }
}
