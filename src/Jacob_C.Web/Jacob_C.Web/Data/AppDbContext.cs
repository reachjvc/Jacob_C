using Microsoft.EntityFrameworkCore;

namespace Jacob_C.Web.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}
