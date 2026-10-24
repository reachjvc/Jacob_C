using Microsoft.EntityFrameworkCore;

namespace Jacob_C.Web.Features.Examples;

public sealed class ExampleDbContext(DbContextOptions<ExampleDbContext> options) : DbContext(options)
{
    public DbSet<ExampleDomain> Examples => Set<ExampleDomain>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExampleDomain>(example =>
        {
            example.HasKey(item => item.Id);
            example.Property(item => item.Name).IsRequired();
            example.Property(item => item.Email).IsRequired();
        });
    }
}
