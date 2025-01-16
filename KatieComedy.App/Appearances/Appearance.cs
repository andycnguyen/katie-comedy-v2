namespace KatieComedy.App.Appearances;

public record Appearance
{
    public int Id { get; init; }

    public required DateTimeOffset DateTime { get; init; }

    public required string LocationName { get; init; }

    public string? LocationUrl { get; init; }

    public required string EventName { get; init; }

    public string? EventUrl { get; init; }
}
