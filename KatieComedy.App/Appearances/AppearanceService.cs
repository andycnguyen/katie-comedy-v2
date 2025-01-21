namespace KatieComedy.App.Appearances;

public class AppearanceService(ApplicationDbContext dbContext)
{
    public async Task<IReadOnlyList<Appearance>> GetUpcoming(CancellationToken cancel)
    {
        return await dbContext.Appearances
            .Where(x => x.DateTime > DateTimeOffset.Now.Date)
            .OrderBy(x => x.DateTime)
            .Select(x => new Appearance
            {
                Id = x.Id,
                DateTime = x.DateTime,
                EventName = x.EventName,
                EventUrl = x.EventUrl,
                VenueName = x.LocationName,
                VenueUrl = x.LocationUrl
            })
            .ToListAsync(cancel);
    }

    public async Task<IReadOnlyList<Appearance>> Get(CancellationToken cancel)
    {
        return await dbContext.Appearances
            .OrderByDescending(x => x.DateTime)
            .Select(x => new Appearance
            {
                Id = x.Id,
                DateTime = x.DateTime,
                EventName = x.EventName,
                EventUrl = x.EventUrl,
                VenueName = x.LocationName,
                VenueUrl = x.LocationUrl
            })
            .ToListAsync(cancel);
    }

    public async Task<Appearance> Get(int id)
    {
        var appearance = await dbContext.Appearances.FindAsync(id);

        if (appearance is null)
        {
            throw new Exception("Appearance not found.");
        }

        return new Appearance
        {
            Id = id,
            DateTime = appearance.DateTime,
            EventName = appearance.EventName,
            EventUrl = appearance.EventUrl,
            VenueName = appearance.LocationName,
            VenueUrl = appearance.LocationUrl
        };
    }

    public async Task AddOrUpdate(Appearance appearance)
    {
        dbContext.Appearances.Update(new Database.Appearance
        {
            Id = appearance.Id,
            DateTime = appearance.DateTime,
            EventName = appearance.EventName,
            EventUrl = appearance.EventUrl,
            LocationName = appearance.VenueName,
            LocationUrl = appearance.VenueUrl
        });

        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        await dbContext.Appearances
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }
}
