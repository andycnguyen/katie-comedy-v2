using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace KatieComedy.App.Email;

public record EmailMessage
{
    public required string ToAddress { get; init; }
    public required string ToName { get; init; }
    public string? ReplyAddress { get; init; }
    public string? ReplyName { get; init; }
    public required string Subject { get; init; }
    public required string HtmlText { get; init; }
}

public class EmailSender(IOptions<EmailOptions> options) : IEmailSender
{
    private readonly EmailOptions _options = options.Value;

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailMessage = new EmailMessage
        {
            ToAddress = email,
            ToName = email.Split('@').FirstOrDefault() ?? string.Empty,
            Subject = subject,
            HtmlText = htmlMessage
        };

        return SendEmailAsync(emailMessage, default);
    }

    public async Task SendEmailAsync(EmailMessage emailMessage, CancellationToken cancel)
    {
        var message = new MimeMessage
        {
            Subject = emailMessage.Subject
        };

        message.From.Add(new MailboxAddress(_options.FromName, _options.FromAddress));
        message.To.Add(new MailboxAddress(emailMessage.ToName, emailMessage.ToAddress));

        if (!string.IsNullOrEmpty(emailMessage.ReplyAddress))
        {
            message.ReplyTo.Add(new MailboxAddress(emailMessage.ReplyName, emailMessage.ReplyAddress));
        }

        message.Body = new TextPart("html")
        {
            Text = emailMessage.HtmlText
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_options.SmtpHost, _options.SmtpPort, true, cancel);
        await client.AuthenticateAsync(_options.SmtpUsername, _options.SmtpPassword, cancel);
        await client.SendAsync(message, cancel);
        await client.DisconnectAsync(true, cancel);
    }
}
