namespace KatieComedy.Web.Pages.Admin;

public class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        return RedirectToPage("Appearances/Index");
    }
}
