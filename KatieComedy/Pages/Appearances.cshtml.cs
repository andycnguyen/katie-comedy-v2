using KatieComedy.App.Appearances;
using KatieComedy.App.Pagination;

namespace KatieComedy.Web.Pages;

public class AppearancesModel(AppearanceService service) : PageModel
{
    public IReadOnlyList<Appearance> Appearances { get; set; } = [];

    public async Task OnGetAsync(CancellationToken cancel)
    {
        var page = await service.GetUpcoming(new PageArgs
        {
            PageIndex = 1,
            PageSize = int.MaxValue
        }, cancel);

        Appearances = page.Items;
    }
}
