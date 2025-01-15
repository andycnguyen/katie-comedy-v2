using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using KatieComedy.App.Email;

namespace KatieComedy.App.Test;

public class EmailTests
{
    [Fact]
    public async Task EmailSender()
    {
        var env = new Mock<IWebHostEnvironment>();
        env.Setup(x => x.IsDevelopment()).Returns(true);

        var options = Options.Create(new EmailOptions
        {
            FromAddress = "katie@katienguyen.com",
            FromName = "Unit Test",
            ContactFormAddress = "badandytowin@gmail.com",
            SmtpHost = "smtp.titan.email",
            SmtpPort = 465,
            SmtpUsername = "katie@katienguyen.com",
            SmtpPassword = "",
            EmailUrl = "https://hostinger.titan.email/login/",
            TestAddress = "badandytowin@gmail.com"
        });

        var sender = new EmailSender(options);

        await sender.SendEmailAsync(new EmailRequest
        {
            ToAddress = "badandytowin@gmail.com",
            ToName = "Andy",
            Subject = "Unit Test Email",
            HtmlText = "<em>This is a test</em>"
        }, default);
    }
}
