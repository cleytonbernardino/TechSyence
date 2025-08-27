using Microsoft.EntityFrameworkCore;
using TechSyence.Domain.Entities;

namespace TechSyence.Infrastructure.DataAccess;

public class TechSyenceDbContext(
    DbContextOptions dbContextOptions
    ) : DbContext(dbContextOptions)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechSyenceDbContext).Assembly);
    }
}
