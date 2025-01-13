namespace KatieComedy.Web;

public record SiteOptions
{
    public const string Section = "Site";

    public bool RedirectToLegacySite { get; init; }
}
