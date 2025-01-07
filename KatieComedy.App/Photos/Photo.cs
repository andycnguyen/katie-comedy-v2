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

    public string DisplayText
    {
        get
        {
            var text = string.Empty;

            if (Date.HasValue)
            {
                text += Date.Value.ToString("d/M/yy");
            }

            if (!string.IsNullOrEmpty(Caption))
            {
                text = $"{text} {Caption}".Trim();
            }

            if (!string.IsNullOrEmpty(Credit))
            {
                text = $"{text} (Credit: {Credit})".Trim();
            }

            return text;
        }
    }
}
