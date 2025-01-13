namespace KatieComedy.Web.Pages;

public class IndexModel(IOptions<SiteOptions> siteOptions) : PageModel
{
    private readonly SiteOptions _siteOptions = siteOptions.Value;

    public IActionResult OnGet()
    {
        if (_siteOptions.RedirectToLegacySite)
        {
            return Redirect(_siteOptions.LegacySiteUrl);
        }

        return Page();
    }
}
