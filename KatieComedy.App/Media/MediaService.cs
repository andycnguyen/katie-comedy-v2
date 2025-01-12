namespace KatieComedy.App.Media;

public class MediaService(ApplicationDbContext dbContext)
{
    public async Task<IReadOnlyList<Media>> Get(CancellationToken cancel)
    {
        return await dbContext.Media
            .OrderBy(x => x.Type)
            .Select(x => new Media
            {
                Id = x.Id,
                Type = x.Type,
                Title = x.Title,
                Url = x.Url
            })
            .ToListAsync(cancel);
    }

    public async Task<Media> Get(int id)
    {
        var media = await dbContext.Media.FindAsync(id);

        if (media is null)
        {
            throw new Exception("Media not found.");
        }

        return new Media
        {
            Id = id,
            Type = media.Type,
            Title = media.Title,
            Url = media.Url
        };
    }

    public async Task AddOrUpdate(Media media)
    {
        dbContext.Media.Update(new Database.Media
        {
            Id = media.Id,
            Type = media.Type,
            Title = media.Title,
            Url = media.Url
        });

        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        await dbContext.Media
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }
}
