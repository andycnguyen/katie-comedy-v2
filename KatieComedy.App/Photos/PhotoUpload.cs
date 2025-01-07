namespace KatieComedy.App.Photos;

public record PhotoUpload
{
    public required byte[] Data { get; init; }

    public required string FileExtension { get; init; }

    public DateOnly? Date { get; init; }

    public string? Caption { get; init; }

    public string? Credit { get; init; }
}
