using KatieComedy.App;
using KatieComedy.App.Media;

namespace KatieComedy.Web.Pages.Admin.Media;

public class EditModel(MediaService service) : BasePageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty, Required]
    public MediaType? Type { get; set; }

    [BindProperty, Required, MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    [BindProperty, Display(Name = "Url"), MaxLength(500), RegularExpression(Constants.UrlPattern, ErrorMessage = "Invalid Url.")]
    public string MediaUrl { get; set; } = string.Empty;

    public async Task OnGetAsync()
    {
        var media = await service.Get(Id);
        Type = media.Type;
        Title = media.Title;
        MediaUrl = media.Url;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await service.AddOrUpdate(new App.Media.Media
        {
            Id = Id,
            Type = Type!.Value,
            Title = Title,
            Url = MediaUrl
        });

        Toast(ToastLevel.Success, "Media updated");
        return RedirectToPage("Index");
    }

    public async Task<IActionResult> OnPostDelete()
    {
        await service.Delete(Id);
        Toast(ToastLevel.Success, "Media deleted");
        return RedirectToPage("Index");
    }
}
