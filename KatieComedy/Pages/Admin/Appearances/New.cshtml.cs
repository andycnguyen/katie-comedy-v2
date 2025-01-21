using KatieComedy.App;
using KatieComedy.App.Appearances;

namespace KatieComedy.Web.Pages.Admin.Appearances;

public class NewModel(AppearanceService service) : BasePageModel
{
    [BindProperty, Required, Display(Name = "Date"), DataType(DataType.DateTime)]
    public DateTime? DateTime { get; set; }

    [BindProperty, Display(Name = "Event Name"), MaxLength(500)]
    public string EventName { get; set; }

    [BindProperty, Display(Name = "Event Url"), MaxLength(500), RegularExpression(Constants.UrlPattern, ErrorMessage = "Invalid Url.")]
    public string? EventUrl { get; set; }

    [BindProperty, Display(Name = "Venue Name"), MaxLength(500)]
    public string VenueName { get; set; }

    [BindProperty, Display(Name = "Venue Url"), MaxLength(500), RegularExpression(Constants.UrlPattern, ErrorMessage = "Invalid Url.")]
    public string? VenueUrl { get; set; }

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
            DateTime = DateTime!.Value,
            EventName = EventName,
            EventUrl = EventUrl,
            VenueName = VenueName,
            VenueUrl = VenueUrl
        });

        Toast(ToastLevel.Success, "Appearance added.");
        return RedirectToPage("Index");
    }
}
