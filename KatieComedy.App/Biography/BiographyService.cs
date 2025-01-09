using System.Text.RegularExpressions;

namespace KatieComedy.App.Biography;

public partial class BiographyService(ApplicationDbContext dbContext)
{
    public async Task<string> Get()
    {
        var biography = await dbContext.Biographies.SingleOrDefaultAsync();
        return biography?.HtmlText ?? string.Empty;
    }

    public async Task Update(string text)
    {
        var biography = await dbContext.Biographies.SingleOrDefaultAsync();
        biography ??= dbContext.Biographies.Add(new Database.Biography()).Entity;
        biography.HtmlText = FormatAsHtml(text);

        await dbContext.SaveChangesAsync();
    }

    public static string FormatAsHtml(string text)
    {
        text = LinebreakRegex().Replace(text, "<br/>");

        foreach (var match in LinkRegex().Matches(text).Cast<Match>())
        {
            var href = match.Groups[1].Value;
            var innerHtml = match.Groups[2].Value;
            var link = $"<a href='{href}' target='_blank'>{innerHtml}</a>";
            text = text.Replace(match.Value, link);
        }

        return text;
    }

    [GeneratedRegex("\\[([^[]]+)\\]\\(([^()]+)\\)")]
    private static partial Regex LinkRegex();

    [GeneratedRegex("\n|\r|\r\n")]
    private static partial Regex LinebreakRegex();
}
