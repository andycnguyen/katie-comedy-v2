using System.ComponentModel.DataAnnotations.Schema;

namespace KatieComedy.App.Database;

[Table(nameof(Photo))]
public class Photo
{
    public int Id { get; set; }

    [ForeignKey(nameof(Photo))]
    public int? ThumbnailId { get; set; }

    public Photo? Thumbnail { get; set; }

    public string BlobContainer { get; set; }

    public string BlobName { get; set; }

    public DateOnly? Date { get; set; }

    public string? Caption { get; set; }

    public string? Credit { get; set; }
}
