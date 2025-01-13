namespace KatieComedy.Web.Cloudflare;

public class CloudflareOptions
{
    public const string Section = "Cloudflare";

    [Required]
    public required string TurnstileSiteKey { get; init; }

    [Required]
    public required string TurnstileSecretKey { get; init; }

    [Required]
    public required string SiteVerifyUrl { get; init; }
}
