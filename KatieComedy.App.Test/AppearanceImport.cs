using System.Globalization;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using KatieComedy.App.Database;

namespace KatieComedy.App.Test;

public class AppearanceImport
{
    [Fact]
    public async Task ImportAppearances()
    {
        var newAppearances = GetAppearances();

        var connectionstring = "";
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionstring);
        using var dbContext = new ApplicationDbContext(optionsBuilder.Options);
        var currentAppearances = await dbContext.Appearances.ToListAsync();

        var appearancesToAdd = newAppearances
            .Where(x => !currentAppearances.Any(y => y.DateTime == x.datetime))
            .ToList();

        dbContext.Appearances.AddRange(newAppearances.Select(x => new Database.Appearance
        {
            DateTime = x.datetime,
            LocationName = x.venue_name,
            LocationUrl = x.venue_url,
            EventName = x.event_name,
            EventUrl = x.event_url
        }));

        await dbContext.SaveChangesAsync();
    }

    private static IReadOnlyList<Appearance> GetAppearances()
    {
        using var reader = new StreamReader("v1-appearances.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Appearance>();
        return records.ToList();
    }

    public record Appearance(
        int id,
        DateTime datetime,
        string venue_name,
        string? venue_url,
        string event_name,
        string event_url);
}
