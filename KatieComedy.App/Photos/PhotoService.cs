using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace KatieComedy.App.Photos;

public class PhotoService(
    ApplicationDbContext dbContext,
    IWebHostEnvironment env,
    IHttpContextAccessor httpContextAccessor,
    IOptions<PhotoOptions> options)
{
    private string PhotoDirectoryPath => Path.Combine(env.WebRootPath, "photos");
    private readonly PhotoOptions _options = options.Value;

    public async Task<IReadOnlyList<Photo>> Get(CancellationToken cancel)
    {
        var photos = await dbContext.Photos
            .Include(x => x.Thumbnail)
            .Where(x => x.Type == PhotoType.Photo)
            .ToListAsync(cancel);

        return photos
            .OrderByDescending(x => x.Date.HasValue)
            .ThenByDescending(x => x.Date)
            .ThenByDescending(x => x.Id)
            .Select(x => new Photo
            {
                Id = x.Id,
                Url = GetUrl(x.Filename)!,
                ThumbnailUrl = GetUrl(x.Thumbnail?.Filename)
            })
            .ToList();
    }

    public async Task<Photo> Get(int id)
    {
        var photo = await dbContext.Photos
            .Include(x => x.Thumbnail)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (photo is null)
        {
            throw new Exception("Photo not found.");
        }

        return new Photo
        {
            Id = photo.Id,
            Url = GetUrl(photo.Filename)!,
            ThumbnailUrl = GetUrl(photo.Thumbnail?.Filename)
        };
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

        dbContext.Photos.Add(new Database.Photo
        {
            Type = PhotoType.Photo,
            Filename = photoFilename,
            Date = upload.Date,
            Caption = upload.Caption,
            Thumbnail = new Database.Photo
            {
                Type = PhotoType.Thumbnail,
                Filename = thumbnailFilename
            }
        });

        await dbContext.SaveChangesAsync(cancel);
    }

    public async Task Delete(int id)
    {
        var photo = await dbContext.Photos
            .Include(x => x.Thumbnail)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new FileNotFoundException();

        dbContext.Photos.Remove(photo);
        File.Delete(Path.Combine(PhotoDirectoryPath, photo.Filename));

        if (photo.Thumbnail is not null)
        {
            dbContext.Photos.Remove(photo.Thumbnail);
            File.Delete(Path.Combine(PhotoDirectoryPath, photo.Thumbnail.Filename));
        }

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

        var dirInfo = new DirectoryInfo(PhotoDirectoryPath);

        foreach (var file in dirInfo.GetFiles())
        {
            if (file.Name == _options.TestPhotoFilename)
            {
                return;
            }

            file.Delete();
        }

        foreach (var dir in dirInfo.GetDirectories())
        {
            dir.Delete(true);
        }
    }

    public async Task InitializeTestPhoto()
    {
        if (env.IsProduction())
        {
            return;
        }

        var photoData = File.ReadAllBytes(Path.Join(PhotoDirectoryPath, _options.TestPhotoFilename));

        foreach (var i in Enumerable.Range(0, 5))
        {
            await Upload(new PhotoUpload
            {
                Data = photoData,
                FileExtension = ".jpg",
                Caption = "This a test photo",
                Date = DateOnly.FromDateTime(DateTime.Now)
            }, default);
        }
    }

    private string? GetUrl(string? filename)
    {
        if (string.IsNullOrEmpty(filename) || httpContextAccessor.HttpContext is null)
        {
            return null;
        }

        var httpContext = httpContextAccessor.HttpContext;
        return $"{httpContext.Request.Scheme}://{httpContext.Request.Host.ToUriComponent()}/photos/{filename}";
    }
}
