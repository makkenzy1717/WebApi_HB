using Microsoft.EntityFrameworkCore;
using Test3;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Currency> Currencies { get; set; }

    
}