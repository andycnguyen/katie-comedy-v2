using KatieComedy.App.Photos;

namespace KatieComedy.Web.Pages.Admin.Photos;

public class UploadModel(PhotoService service, IOptions<PhotoOptions> options) : PageModel
{
    private readonly PhotoOptions _options = options.Value;

    [BindProperty]
    public IFormFile File { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancel)
    {
        using var memoryStream = new MemoryStream();
        await File.CopyToAsync(memoryStream, cancel);

        if (memoryStream.Length > Math.Pow(2, 20) * _options.MaxFilesizeMegabytes)
        {
            ModelState.AddModelError(nameof(File), $"File exceeds maximum size of {_options.MaxFilesizeMegabytes} MB.");
        }

        var info = new FileInfo(File.FileName);
        var fileExtension = info.Extension.ToLower();

        if (!options.Value.AllowedFileExtensions.Contains(fileExtension))
        {
            ModelState.AddModelError(nameof(File), "Invalid file extension.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await service.Upload(new PhotoUpload
        {
            Data = memoryStream.ToArray(),
            FileExtension = fileExtension
        }, cancel);

        return RedirectToPage("Index");
    }
}
