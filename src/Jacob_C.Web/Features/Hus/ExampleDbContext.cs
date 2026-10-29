using Microsoft.EntityFrameworkCore;

namespace Jacob_C.Web.Features.Hus;

public sealed class ExampleDbContext(DbContextOptions<ExampleDbContext> options) : DbContext(options)
{
    public DbSet<Hus> Examples => Set<Hus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hus>(example =>
        {
            example.HasKey(item => item.Id);
            example.Property(item => item.Name).IsRequired();
            example.Property(item => item.Kvm).IsRequired();
        });
    }
}
