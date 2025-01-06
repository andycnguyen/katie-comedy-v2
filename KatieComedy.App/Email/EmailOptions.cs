﻿using System.ComponentModel.DataAnnotations;

namespace KatieComedy.App.Email;

public record EmailOptions
{
    public static readonly string Section = nameof(EmailOptions);

    [Required]
    public required string FromName { get; init; }

    [Required]
    public required string FromAddress { get; init; }

    [Required]
    public required string SmtpHost { get; init; }

    [Required]
    public required int SmtpPort { get; init; }

    [Required]
    public required string SmtpUsername { get; init; }

    [Required]
    public required string SmtpPassword { get; init; }
}
