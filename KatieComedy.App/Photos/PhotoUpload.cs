namespace KatieComedy.App.Photos;

public record PhotoUpload
{
    public required byte[] Data { get; init; }

    public required string FileExtension { get; init; }
}
