using KatieComedy.App.Email;
using Microsoft.Extensions.Options;

namespace KatieComedy.App.Test;

public class EmailTests
{
    [Fact]
    public async Task EmailSender()
    {
        var options = Options.Create(new EmailOptions
        {
            FromAddress = "katie@katienguyen.com",
            FromName = "Unit Test",
            SmtpHost = "smtp.titan.email",
            SmtpPort = 465,
            SmtpUsername = "katie@katienguyen.com",
            SmtpPassword = "",
        });

        var sender = new EmailSender(options);

        await sender.SendEmailAsync(new EmailMessage
        {
            ToAddress = "badandytowin@gmail.com",
            ToName = "Andy",
            Subject = "Unit Test Email",
            HtmlText = "<em>This is a test</em>"
        }, default);
    }
}
