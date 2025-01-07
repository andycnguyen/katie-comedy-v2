namespace KatieComedy.App.Photos;

public enum PhotoType
{
    Unknown = 0,
    Photo,
    Thumbnail
}

public record Photo
{
    public required int Id { get; init; }
    public required string Url { get; init; }
    public string? ThumbnailUrl { get; init; }
}
