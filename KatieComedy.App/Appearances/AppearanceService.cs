using KatieComedy.App.Pagination;

namespace KatieComedy.App.Appearances;

public class AppearanceService(ApplicationDbContext dbContext)
{
    public async Task<PagedResult<Appearance>> GetUpcoming(PageArgs args, CancellationToken cancel)
    {
        var query = dbContext.Appearances
            .OrderBy(x => x.DateTime)
            .Where(x => x.DateTime >= DateTimeOffset.Now.Date);

        var appearances = await query
            .Skip((args.PageIndex - 1) * args.PageSize)
            .Take(args.PageSize)
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

        var count = await query.CountAsync(cancel);

        return new PagedResult<Appearance>
        {
            Items = appearances,
            Parameters = new PageParameters
            {
                PageIndex = args.PageIndex,
                PageSize = args.PageSize,
                TotalCount = count
            }
        };
    }

    public async Task<PagedResult<Appearance>> GetArchived(PageArgs args, CancellationToken cancel)
    {
        var query = dbContext.Appearances
            .OrderBy(x => x.DateTime)
            .Where(x => x.DateTime < DateTimeOffset.Now.Date);

        var appearances = await query
            .Skip((args.PageIndex - 1) * args.PageSize)
            .Take(args.PageSize)
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

        var count = await query.CountAsync(cancel);

        return new PagedResult<Appearance>
        {
            Items = appearances,
            Parameters = new PageParameters
            {
                PageIndex = args.PageIndex,
                PageSize = args.PageSize,
                TotalCount = count
            }
        };
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
