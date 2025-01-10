namespace KatieComedy.App.Media;

public enum MediaType
{
    Text = 1,
    Audio = 2,
    Video = 3
}

public record Media
{
    public int Id { get; init; }

    public required MediaType Type { get; init; }

    public required string Title { get; init; }

    public required string Url { get; init; }
}
