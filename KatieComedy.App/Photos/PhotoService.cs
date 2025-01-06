using KatieComedy.App.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace KatieComedy.App.Photos;

public class PhotoService(ApplicationDbContext dbContext, IWebHostEnvironment env)
{
    private const string PhotoDirectory = "photos";

    public async Task<IReadOnlyList<Photo>> Get(CancellationToken cancel)
    {
        var photos = await dbContext.Photos.ToListAsync(cancel);

        return photos
            .OrderByDescending(x => x.Date.HasValue)
            .ThenByDescending(x => x.Date)
            .ThenByDescending(x => x.Id)
            .Select(x => new Photo
            {
                Id = x.Id,
                Url = string.Empty
            })
            .ToList();
    }

    public async Task Upload(PhotoUpload upload, CancellationToken cancel)
    {
        var directory = Path.Combine(env.WebRootPath, PhotoDirectory);
        Directory.CreateDirectory(directory);
        var filename = Guid.NewGuid().ToString() + upload.FileExtension;
        var filepath = Path.Combine(directory, filename);
        await File.WriteAllBytesAsync(filepath, upload.Data, cancel);
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

    public async Task DeleteAll()
    {

    }
}
