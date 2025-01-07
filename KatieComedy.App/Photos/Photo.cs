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

    public DateOnly? Date { get; init; }

    public string? Caption { get; init; }

    public string? Credit { get; init; }
}
