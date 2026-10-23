using Microsoft.EntityFrameworkCore;
using Jacob_C.Web.Features.Examples;

namespace Jacob_C.Web.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ExampleDomain> Examples => Set<ExampleDomain>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExampleDomain>(example =>
        {
            example.HasKey(item => item.Id);
            example.Property(item => item.Name)
                .HasMaxLength(ExampleDomain.MaxNameLength)
                .IsRequired();
        });
    }
}
