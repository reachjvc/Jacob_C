using Microsoft.EntityFrameworkCore;

namespace Jacob_C.Web.Features.Hus;

public sealed class HusDbContext(DbContextOptions<HusDbContext> options) : DbContext(options)
{
    public DbSet<Hus> Huse => Set<Hus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hus>(hus =>
        {
            hus.HasKey(item => item.Id);
            hus.Property(item => item.Name).IsRequired();
            hus.Property(item => item.Kvm).IsRequired();
        });
    }
}
