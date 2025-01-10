using KatieComedy.App.Biography;

namespace KatieComedy.Web.Pages.Admin.Biography;

public class EditModel(BiographyService service) : PageModel
{
    [BindProperty, MaxLength(5000)]
    public string Text { get; set; } = string.Empty;

    public string Html => BiographyService.FormatAsHtml(Text);

    public async Task OnGetAsync()
    {
        Text = await service.Get();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await service.Update(Text);
        // need success feedback here
        return RedirectToPage("Edit");
    }
}
