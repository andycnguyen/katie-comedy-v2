using Microsoft.EntityFrameworkCore;

namespace KatieComedy.App.Database;

public class DatabaseInitializer(ApplicationDbContext dbContext)
{
    public async Task InitializeAsync()
    {
        if (dbContext.Database.GetConnectionString()?.Contains("localdb") ?? false)
        {
            await dbContext.Database.EnsureDeletedAsync();
        }

        await dbContext.Database.MigrateAsync();

        // initialize identity
    }
}
