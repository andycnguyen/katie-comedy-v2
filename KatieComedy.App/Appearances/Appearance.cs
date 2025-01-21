namespace KatieComedy.App.Appearances;

public record Appearance
{
    public int Id { get; init; }

    public required DateTime DateTime { get; init; }

    public required string VenueName { get; init; }

    public string? VenueUrl { get; init; }

    public required string EventName { get; init; }

    public string? EventUrl { get; init; }
}
