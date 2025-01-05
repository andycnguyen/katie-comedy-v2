using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KatieComedy.App.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Appearance> Appearances { get; set; }
    public DbSet<Biography> Biographies { get; set; }
    public DbSet<Media> Media { get; set; }
    public DbSet<Photo> Photos { get; set; }
}
