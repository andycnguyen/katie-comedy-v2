namespace KatieComedy.App.Biography;

public class BiographyService
{
    public async Task<string> Get()
    {
        return string.Empty;
    }

    public async Task Update(string text)
    {
        text = FormatToHtml(text);
    }

    public static string FormatToHtml(string text)
    {
        return text;
    }
}
