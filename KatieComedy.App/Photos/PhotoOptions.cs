using System.ComponentModel.DataAnnotations;

namespace KatieComedy.App.Photos;

public record PhotoOptions
{
    public static readonly string Section = "Photo";

    [Required]
    public int MaxFilesizeMegabytes { get; init; }

    [Required]
    public IReadOnlyList<string> AllowedFileExtensions { get; init; } = [];
}
