using System.Text.Json;

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
