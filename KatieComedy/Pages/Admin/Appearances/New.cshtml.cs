using KatieComedy.App;
using KatieComedy.App.Appearances;

namespace KatieComedy.Web.Pages.Admin.Appearances;

public class NewModel(AppearanceService service) : BasePageModel
{
    [BindProperty, Required, Display(Name = "Date"), DataType(DataType.DateTime)]
    public DateTimeOffset? DateTime { get; set; }

    [BindProperty, Display(Name = "Event Name"), MaxLength(500)]
    public string EventName { get; set; }

    [BindProperty, Display(Name = "Event Url"), MaxLength(500), RegularExpression(Constants.UrlPattern, ErrorMessage = "Invalid Url.")]
    public string? EventUrl { get; set; }

    [BindProperty, Display(Name = "Location Name"), MaxLength(500)]
    public string LocationName { get; set; }

    [BindProperty, Display(Name = "Location Url"), MaxLength(500), RegularExpression(Constants.UrlPattern, ErrorMessage = "Invalid Url.")]
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
            DateTime = DateTime!.Value,
            EventName = EventName,
            EventUrl = EventUrl,
            LocationName = LocationName,
            LocationUrl = LocationUrl
        });

        Toast(ToastLevel.Success, "Appearance added");
        return RedirectToPage("Index");
    }
}
