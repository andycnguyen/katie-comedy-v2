using KatieComedy.App.Biography;

namespace KatieComedy.Web.Pages;

public class AboutModel(BiographyService service) : PageModel
{
    public string Text { get; set; } = string.Empty;

    public string Html => BiographyService.FormatAsHtml(Text);

    public async Task OnGet()
    {
        Text = await service.Get();
    }
}
