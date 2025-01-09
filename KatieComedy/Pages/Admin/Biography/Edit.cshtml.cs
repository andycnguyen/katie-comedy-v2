using KatieComedy.App.Biography;

namespace KatieComedy.Web.Pages.Admin.Biography;

public class EditModel(BiographyService service) : PageModel
{
    [MinLength(1), MaxLength(5000)]
    public string Text { get; set; } = string.Empty;

    public async Task OnGetAsync()
    {
        Text = await service.Get();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Text.Trim().Length == 0)
        {
            ModelState.AddModelError(nameof(Text), "Text cannot be empty.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await service.Update(Text);
        // need success feedback here
        return RedirectToPage("Edit");
    }
}
