using KatieComedy.App.Appearances;

namespace KatieComedy.Web.Pages.Admin.Appearances;

public class EditModel(AppearanceService service) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty, Display(Name = "Date"), DataType(DataType.DateTime)]
    public DateTimeOffset DateTime { get; set; }

    [BindProperty, Display(Name = "Event Name"), MaxLength(500)]
    public string EventName { get; set; }

    [BindProperty, Display(Name = "Event Url"), MaxLength(500), RegularExpression(UrlRegex, ErrorMessage = "Invalid Url.")]
    public string? EventUrl { get; set; }

    [BindProperty, Display(Name = "Location Name"), MaxLength(500)]
    public string LocationName { get; set; }

    [BindProperty, Display(Name = "Location Url"), MaxLength(500), RegularExpression(UrlRegex, ErrorMessage = "Invalid Url.")]
    public string? LocationUrl { get; set; }

    private const string UrlRegex = "^((https?|ftp|smtp):\\/\\/)?(www.)?[a-z0-9]+\\.[a-z]+(\\/[a-zA-Z0-9#]+\\/?)*$";

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
