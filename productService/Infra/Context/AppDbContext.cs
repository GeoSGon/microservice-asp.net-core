using Microsoft.EntityFrameworkCore;
using productService.Domain.Entities;

namespace productService.Infra.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)  
        : base(options) 
    {}

    public DbSet<Product>? Products { get; set; }
    public DbSet<Category>? Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Category>(c =>
        {
            c.HasKey(c => c.Id);
            c.Property(c => c.Name).HasMaxLength(60).IsRequired();
            c.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); 
        });

        mb.Entity<Product>(p =>
        {;
            p.HasKey(p => p.Id);
            p.Property(p => p.Name).HasMaxLength(60).IsRequired();
            p.Property(p => p.Price).HasPrecision(12, 2);
            p.Property(p => p.Description).HasMaxLength(1024).IsRequired();
        });
    }
}