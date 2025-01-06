using KatieComedy.App.Database;

namespace KatieComedy.App.Appearances;

public class AppearanceService(ApplicationDbContext dbContext)
{
    public async Task<IReadOnlyList<Appearance>> Get(CancellationToken cancel)
    {
        return null;
    }

    public async Task<Appearance> Get(int id)
    {
        return null;
    }

    public async Task New()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task Update()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        await dbContext.SaveChangesAsync();
    }
}
