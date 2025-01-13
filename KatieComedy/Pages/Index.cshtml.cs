namespace KatieComedy.Web.Pages;

public class IndexModel(IOptions<SiteOptions> siteOptions) : PageModel
{
    public IActionResult OnGet()
    {
        if (siteOptions.Value.RedirectToLegacySite)
        {
            return Redirect("https://katie-nguyen.com");
        }

        return Page();
    }
}
