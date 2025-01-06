using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using KatieComedy.App.Database;
using Microsoft.Extensions.Hosting;

namespace KatieComedy.App.Photos;

public class PhotoService(
    ApplicationDbContext dbContext,
    IWebHostEnvironment env,
    IOptions<PhotoOptions> options)
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
        var photoFilename = Guid.NewGuid().ToString() + upload.FileExtension;
        var photoFilepath = Path.Combine(directory, photoFilename);
        await File.WriteAllBytesAsync(photoFilepath, upload.Data, cancel);

        using var image = await Image.LoadAsync(photoFilepath, cancel);
        var length = Math.Min(image.Width, image.Height);
        image.Mutate(x => x
            .Crop(new Rectangle(0, 0, length, length))
            .Resize(options.Value.ThumbnailLength, options.Value.ThumbnailLength));
        var thumbnailFilename = Guid.NewGuid().ToString() + ".jpg";
        var thumbnailFilepath = Path.Combine(directory, thumbnailFilename);
        await image.SaveAsJpegAsync(thumbnailFilepath, cancel);
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

    public void DeleteAll()
    {
        if (env.IsProduction())
        {
            return;
        }

        var dirInfo = new DirectoryInfo(Path.Combine(env.WebRootPath, PhotoDirectory));

        foreach (var file in dirInfo.GetFiles())
        {
            file.Delete();
        }

        foreach (var dir in dirInfo.GetDirectories())
        {
            dir.Delete(true);
        }
    }
}
