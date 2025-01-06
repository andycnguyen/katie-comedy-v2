using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace KatieComedy.App.Photos;

public class PhotoService(
    ApplicationDbContext dbContext,
    IWebHostEnvironment env,
    IOptions<PhotoOptions> options)
{
    private string PhotoDirectoryPath => Path.Combine(env.WebRootPath, PhotoDirectoryPath);
    private readonly PhotoOptions _options = options.Value;

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
        Directory.CreateDirectory(PhotoDirectoryPath);
        var photoFilename = Guid.NewGuid().ToString() + upload.FileExtension;
        var photoFilepath = Path.Combine(PhotoDirectoryPath, photoFilename);
        await File.WriteAllBytesAsync(photoFilepath, upload.Data, cancel);

        using var image = await Image.LoadAsync(photoFilepath, cancel);
        var length = Math.Min(image.Width, image.Height);
        image.Mutate(x => x
            .Crop(new Rectangle(0, 0, length, length))
            .Resize(_options.ThumbnailLength, _options.ThumbnailLength));
        var thumbnailFilename = Guid.NewGuid().ToString() + ".jpg";
        var thumbnailFilepath = Path.Combine(PhotoDirectoryPath, thumbnailFilename);
        await image.SaveAsJpegAsync(thumbnailFilepath, cancel);
    }

    public async Task Delete(int id)
    {
        var photo = await dbContext.Photos.FindAsync(id) ?? throw new FileNotFoundException();
        var filepath = Path.Combine(PhotoDirectoryPath, photo.Filename);
        File.Delete(filepath);
        dbContext.Photos.Remove(photo);
        await dbContext.SaveChangesAsync();

        // delete thumbnail too
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

        var dirInfo = new DirectoryInfo(PhotoDirectoryPath);

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
