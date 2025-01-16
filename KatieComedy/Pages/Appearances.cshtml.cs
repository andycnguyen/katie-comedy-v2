using KatieComedy.App.Appearances;

namespace KatieComedy.Web.Pages;

public class AppearancesModel(AppearanceService service) : PageModel
{
    public IReadOnlyList<Appearance> Appearances { get; set; } = [];

    public async Task OnGetAsync(CancellationToken cancel)
    {
        Appearances = await service.GetUpcoming(cancel);
    }
}
