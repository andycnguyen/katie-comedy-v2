using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace KatieComedy.App.Database;

public class DatabaseInitializer(
    IWebHostEnvironment env,
    ApplicationDbContext dbContext)
{
    public async Task InitializeAsync()
    {
        if (dbContext.Database.GetConnectionString()?.Contains("localdb") ?? false)
        {
            await dbContext.Database.EnsureDeletedAsync();
        }

        await dbContext.Database.MigrateAsync();
        await InitializeTestEntities();
    }

    private async Task InitializeTestEntities()
    {
        if (!env.IsDevelopment())
        {
            return;
        }

        var random = new Random();

        dbContext.Appearances.AddRange(Enumerable.Range(1, 25).Select(x => new Appearance
        {
            DateTime = DateTime.Now.AddDays(random.Next(0, 30)).AddMinutes(random.Next(0, 360)),
            EventName = "Test event",
            EventUrl = "http://nytimes.com",
            LocationName = "Test venue",
            LocationUrl = null
        }));

        dbContext.Appearances.AddRange(Enumerable.Range(1, 50).Select(x => new Appearance
        {
            DateTime = DateTime.Now.AddDays(-random.Next(0, 30)).AddMinutes(random.Next(0, 360)),
            EventName = "Test event",
            EventUrl = "http://nytimes.com",
            LocationName = "Test venue",
            LocationUrl = null
        }));

        dbContext.Biographies.Add(new Biography
        {
            Text =
            """
            This is my biography.
            
            This is a newline.
            
            This is a [link](http://nytimes.com).
            """
        });

        dbContext.Media.AddRange(
        [
            new Media
            {
                Type = App.Media.MediaType.Text,
                Title = "Text Media",
                Url = "http://youtube.com"
            },
            new Media
            {
                Type = App.Media.MediaType.Audio,
                Title = "Audio Media",
                Url = "http://youtube.com"
            },
            new Media
            {
                Type = App.Media.MediaType.Video,
                Title = "Youtube Media",
                Url = "https://www.youtube.com/watch?v=0Z2veZAlkYM&t"
            },
            new Media
            {
                Type = App.Media.MediaType.Video,
                Title = "Video Media",
                Url = "https://www.youtube.com/watch?v=0Z2veZAlkYM&t"
            }
        ]);

        await dbContext.SaveChangesAsync();
    }
}
