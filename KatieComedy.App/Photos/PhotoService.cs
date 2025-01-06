using KatieComedy.App.Database;
using Microsoft.AspNetCore.Hosting;

namespace KatieComedy.App.Photos;

public class PhotoService(ApplicationDbContext dbContext, IWebHostEnvironment env)
{
    private const string PhotoDirectory = "photos";

    public async Task<IReadOnlyList<Photo>> Get(CancellationToken cancel)
    {
        return [];
    }

    public async Task New(CancellationToken cancel)
    {
        var filename = Guid.NewGuid().ToString();
        var filepath = Path.Combine(env.WebRootPath, PhotoDirectory, filename);
        Directory.CreateDirectory(filepath);
    }

    public async Task Delete(int id)
    {
        var photo = await dbContext.Photos.FindAsync(id) ?? throw new FileNotFoundException();
        var filepath = Path.Combine(env.WebRootPath, PhotoDirectory, photo.Filename);
        File.Delete(filepath);
        dbContext.Photos.Remove(photo);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(int id)
    {

    }
}
