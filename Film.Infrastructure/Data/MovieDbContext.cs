using Film.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Film.Infrastructure.Data;

public class MovieDbContext(DbContextOptions<MovieDbContext> options):DbContext(options)
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AppUser>()
            .Property(tmp=>tmp.Role)
            .HasConversion<string>(); 
    }
}