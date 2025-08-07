using Microsoft.EntityFrameworkCore;
using TechSyence.Domain.Entities;

namespace TechSyence.Infrastructure.DataAccess;

public class TechSyenceDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public TechSyenceDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) {  }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechSyenceDbContext).Assembly);
    }
}
