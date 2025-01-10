using KatieComedy.App;
using KatieComedy.App.Media;

namespace KatieComedy.Web.Pages.Admin.Media;

public class NewModel(MediaService service) : PageModel
{
    [BindProperty, Required]
    public MediaType? Type { get; set; }

    [BindProperty, Required, MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    [BindProperty, Display(Name = "Url"), MaxLength(500), RegularExpression(Constants.UrlPattern, ErrorMessage = "Invalid Url.")]
    public string MediaUrl { get; set; } = string.Empty;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await service.AddOrUpdate(new App.Media.Media
        {
            Type = Type!.Value,
            Title = Title,
            Url = MediaUrl
        });

        return RedirectToPage("Index");
    }
}
