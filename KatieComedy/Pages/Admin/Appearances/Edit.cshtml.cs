using KatieComedy.App;
using KatieComedy.App.Appearances;

namespace KatieComedy.Web.Pages.Admin.Appearances;

public class EditModel(AppearanceService service) : BasePageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty, Required, Display(Name = "Date"), DataType(DataType.DateTime)]
    public DateTime? DateTime { get; set; }

    [BindProperty, Display(Name = "Event Name"), MaxLength(500)]
    public string EventName { get; set; }

    [BindProperty, Display(Name = "Event Url"), MaxLength(500), RegularExpression(Constants.UrlPattern, ErrorMessage = "Invalid Url.")]
    public string? EventUrl { get; set; }

    [BindProperty, Display(Name = "Location Name"), MaxLength(500)]
    public string LocationName { get; set; }

    [BindProperty, Display(Name = "Location Url"), MaxLength(500), RegularExpression(Constants.UrlPattern, ErrorMessage = "Invalid Url.")]
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
            Id = Id,
            DateTime = DateTime!.Value,
            EventName = EventName,
            EventUrl = EventUrl,
            LocationName = LocationName,
            LocationUrl = LocationUrl
        });


        Toast(ToastLevel.Success, "Appearance updated");
        return RedirectToPage("Index");
    }

    public async Task<IActionResult> OnPostDelete()
    {
        await service.Delete(Id);
        Toast(ToastLevel.Success, "Appearance deleted");
        return RedirectToPage("Index");
    }
}
