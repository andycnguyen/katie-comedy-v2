using KatieComedy.App.Appearances;
using KatieComedy.App.Pagination;

namespace KatieComedy.Web.Pages.Admin.Appearances;

public class IndexModel(AppearanceService service) : BasePageModel
{
    [FromQuery]
    public int PageIndex { get; set; }

    [FromQuery]
    public int PageSize { get; set; }

    public PagedResult<Appearance> Appearances { get; set; }

    public async Task OnGetAsync(CancellationToken cancel)
    {
        Appearances = await service.GetUpcoming(new PageArgs
        {
            PageIndex = Math.Max(1, PageIndex),
            PageSize = Math.Max(10, PageSize)
        }, cancel);
    }
}
