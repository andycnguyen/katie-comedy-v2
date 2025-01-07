using KatieComedy.App.Photos;

namespace KatieComedy.Web.Pages.Admin.Photos;

public class EditModel(PhotoService service) : PageModel
{
    public Photo Photo { get; set; }

    public async Task OnGetAsync(int id)
    {
        Photo = await service.Get(id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        return Page();
    }
}
