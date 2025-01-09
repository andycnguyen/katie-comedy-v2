using KatieComedy.App.Appearances;

namespace KatieComedy.Web.Pages.Admin.Appearances;

public class NewModel(AppearanceService service) : PageModel
{
    [BindProperty]
    public DateTimeOffset DateTime { get; set; }

    [BindProperty, MaxLength(500)]
    public string EventName { get; set; }

    [BindProperty, MaxLength(500)]
    public string? EventUrl { get; set; }

    [BindProperty, MaxLength(500)]
    public string LocationName { get; set; }

    [BindProperty, MaxLength(500)]
    public string? LocationUrl { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await service.AddOrUpdate(new Appearance
        {
            DateTime = DateTime,
            EventName = EventName,
            EventUrl = EventUrl,
            LocationName = LocationName,
            LocationUrl = LocationUrl
        });

        return RedirectToPage("Index");
    }
}
