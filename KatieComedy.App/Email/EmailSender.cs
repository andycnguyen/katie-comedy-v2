using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace KatieComedy.App.Email;

public record EmailRequest
{
    public required string ToAddress { get; init; }
    public required string ToName { get; init; }
    public string? ReplyAddress { get; init; }
    public string? ReplyName { get; init; }
    public required string Subject { get; init; }
    public required string HtmlText { get; init; }
}

public class EmailSender(IWebHostEnvironment env, IOptions<EmailOptions> options) : IEmailSender
{
    private readonly EmailOptions _options = options.Value;

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var request = new EmailRequest
        {
            ToAddress = email,
            ToName = email.Split('@').FirstOrDefault() ?? string.Empty,
            Subject = subject,
            HtmlText = htmlMessage
        };

        return SendEmailAsync(request, default);
    }

    public async Task SendEmailAsync(EmailRequest request, CancellationToken cancel)
    {
        var message = new MimeMessage
        {
            Subject = request.Subject
        };

        message.From.Add(new MailboxAddress(_options.FromName, _options.FromAddress));
        var toAddress = env.IsDevelopment() ? _options.TestAddress : request.ToAddress;
        message.To.Add(new MailboxAddress(request.ToName, toAddress));

        if (!string.IsNullOrEmpty(request.ReplyAddress))
        {
            message.ReplyTo.Add(new MailboxAddress(request.ReplyName, request.ReplyAddress));
        }

        message.Body = new TextPart("html")
        {
            Text = request.HtmlText
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_options.SmtpHost, _options.SmtpPort, true, cancel);
        await client.AuthenticateAsync(_options.SmtpUsername, _options.SmtpPassword, cancel);
        await client.SendAsync(message, cancel);
        await client.DisconnectAsync(true, cancel);
    }
}
