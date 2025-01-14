namespace KatieComedy.Web;

public record SiteOptions
{
    public const string Section = "Site";

    public bool RedirectToLegacySite { get; init; }

    [Required]
    public required string LegacySiteUrl { get; init; }

    [Required]
    public required string BlueskyUrl { get; init; }

    [Required]
    public required string FacebookUrl { get; init; }

    [Required]
    public required string InstagramUrl { get; init; }
}
