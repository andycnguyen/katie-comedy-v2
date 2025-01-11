using System.Text.Json;
using KatieComedy.Web.Pages.Shared;

namespace KatieComedy.Web.Pages;

public class BasePageModel : PageModel
{
    public void Toast(ToastLevel level, string message)
    {
        TempData[nameof(ToastModel)] = JsonSerializer.Serialize(new ToastModel
        {
            Level = level,
            Message = message
        });
    }
}
