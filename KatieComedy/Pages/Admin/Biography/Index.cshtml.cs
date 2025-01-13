namespace KatieComedy.Web.Pages.Admin.Biography;

public class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        return RedirectToPage("Edit");
    }
}
