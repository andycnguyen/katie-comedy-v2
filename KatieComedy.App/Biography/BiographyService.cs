using System.Text.RegularExpressions;

namespace KatieComedy.App.Biography;

public class BiographyService(ApplicationDbContext dbContext)
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
        text = Regex.Replace(text, "\n|\r|\r\n", "<br/>");
        return text;
    }
}
