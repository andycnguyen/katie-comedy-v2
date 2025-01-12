using System.ComponentModel.DataAnnotations;

namespace KatieComedy.App.Identity;

public record IdentityOptions
{
    public const string Section = "Identity";

    [Required]
    public required string DefaultAdminEmail { get; init; }

    [Required]
    public required string DefaultAdminPassword { get; init; }

    [Required]
    public required string GoogleClientId { get; init; }

    [Required]
    public required string GoogleSecret { get; init; }
}
