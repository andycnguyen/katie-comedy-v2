using Ganss.Xss;

namespace KatieComedy.App;

public static class Utilities
{
    private static readonly HtmlSanitizer _htmlSanitizer = new();

    public static string SanitizeHtml(string html) => _htmlSanitizer.Sanitize(html);
}
