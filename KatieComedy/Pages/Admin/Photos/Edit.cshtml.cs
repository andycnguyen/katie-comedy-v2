using KatieComedy.App.Photos;

namespace KatieComedy.Web.Pages.Admin.Photos;

public class EditModel(PhotoService service) : BasePageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public DateOnly? Date { get; set; }

    [BindProperty, MaxLength(500)]
    public string? Caption { get; set; }

    [BindProperty, MaxLength(500)]
    public string? Credit { get; set; }

    public async Task OnGet()
    {
        var photo = await service.Get(Id);

        Date = photo.Date;
        Caption = photo.Caption;
        Credit = photo.Credit;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await service.Update(new PhotoUpdate
        {
            Id = Id,
            Date = Date,
            Caption = Caption,
            Credit = Credit
        });


        Toast(ToastLevel.Success, "Photo updated");
        return RedirectToPage("Index");
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        await service.Delete(Id);
        Toast(ToastLevel.Success, "Photo deleted");
        return RedirectToPage("Index");
    }
}
