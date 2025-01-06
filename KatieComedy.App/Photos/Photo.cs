namespace KatieComedy.App.Photos;

public enum PhotoType
{
    Photo,
    Thumbnail
}

public record Photo
{
    public required int Id { get; init; }
    public required string Url { get; init; }
}
