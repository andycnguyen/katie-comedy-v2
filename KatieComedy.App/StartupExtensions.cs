using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.UI.Services;
using KatieComedy.App.Email;
using KatieComedy.App.Photos;
using KatieComedy.App.Appearances;
using KatieComedy.App.Database;

namespace KatieComedy.App;

public static class StartupExtensions
{
    public static WebApplicationBuilder AddAppServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<DatabaseInitializer>()
            .AddScoped<EmailSender>()
            .AddScoped<IEmailSender, EmailSender>()
            .AddScoped<PhotoService>()
            .AddScoped<AppearanceService>();

        builder.Services
            .Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.Section))
            .Configure<PhotoOptions>(builder.Configuration.GetSection(PhotoOptions.Section));

        return builder;
    }
}
