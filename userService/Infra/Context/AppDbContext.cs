using Microsoft.EntityFrameworkCore;
using userService.Domain.Entities;

namespace userService.Infra.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)  
        : base(options) 
    {}
    public DbSet<User>? Users { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<User>(u =>
        {
            u.HasKey(u => u.Id);
            u.Property(u => u.Username).HasMaxLength(60).IsRequired();
            u.Property(u => u.Password).HasMaxLength(60).IsRequired();
        });
    }
}