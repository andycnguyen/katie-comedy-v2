using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

namespace KatieComedy.Web.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel(ILogger<ErrorModel> logger) : PageModel
{
    public string? RequestId => Activity.Current?.Id ?? HttpContext.TraceIdentifier;

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public void OnGet()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (exception is not null)
        {
            logger.LogError(exception.Error, $"Error: {exception.Error.Message}");
        }
    }
}
