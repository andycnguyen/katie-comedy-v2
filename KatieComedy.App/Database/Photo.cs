using System.ComponentModel.DataAnnotations.Schema;
using KatieComedy.App.Photos;

namespace KatieComedy.App.Database;

[Table(nameof(Photo))]
public class Photo
{
    public int Id { get; set; }

    public PhotoType Type { get; set; }

    [ForeignKey(nameof(Photo))]
    public int? ThumbnailId { get; set; }

    public Photo? Thumbnail { get; set; }

    public string Filename { get; set; }

    public DateOnly? Date { get; set; }

    public string? Caption { get; set; }

    public string? Credit { get; set; }
}
