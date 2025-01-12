using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KatieComedy.App;
using KatieComedy.App.Database;
using KatieComedy.App.Photos;
using KatieComedy.App.Biography;
using KatieComedy.App.Identity;
using static KatieComedy.App.Constants;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddHttpContextAccessor()
    .AddTransient<IActionContextAccessor, ActionContextAccessor>()
    .AddScoped<IUrlHelper>(services =>
    {
        var actionContext = services
            .GetRequiredService<IActionContextAccessor>()
            .ActionContext;
        return new UrlHelper(actionContext!);
    })
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddRazorPages(options =>
    {
        options.Conventions.AuthorizeFolder("/admin", AuthPolicy.Admin);
        options.Conventions.AuthorizeFolder("/admin/users", AuthPolicy.Owner);
        options.Conventions.AllowAnonymousToPage("/admin/users/register");
    });

builder.Services
    .AddAuthorizationBuilder()
    .AddPolicy(AuthPolicy.Admin, options =>
    {
        options.RequireRole([Roles.Admin, Roles.Owner]);
    })
    .AddPolicy(AuthPolicy.Owner, options =>
    {
        options.RequireRole([Roles.Owner]);
    });

builder.AddAppServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app
    .UseHttpsRedirection()
    .UseRouting()
    .UseAuthorization()
    .UseStaticFiles();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapGet("admin/api/biography", ([FromQuery] string text) => BiographyService.FormatAsHtml(text));

await InitializeApp();

app.Run();

async Task InitializeApp()
{
    using var scope = app.Services.CreateScope();
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await dbInitializer.InitializeAsync();

    var photoService = scope.ServiceProvider.GetRequiredService<PhotoService>();
    photoService.DeleteAll();
    await photoService.InitializeTestPhoto();

    var identityInitializer = scope.ServiceProvider.GetRequiredService<IdentityInitializer>();
    await identityInitializer.Initialize();
}
