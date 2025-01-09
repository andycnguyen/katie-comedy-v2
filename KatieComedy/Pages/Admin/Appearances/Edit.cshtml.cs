using KatieComedy.App.Appearances;

namespace KatieComedy.Web.Pages.Admin.Appearances;

public class EditModel(AppearanceService service) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

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

    public async Task OnGetAsync()
    {
        var appearance = await service.Get(Id);

        DateTime = appearance.DateTime;
        EventName = appearance.EventName;
        EventUrl = appearance.EventUrl;
        LocationName = appearance.LocationName;
        LocationUrl = appearance.LocationUrl;
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
