using TechSyence.Domain.Repositories;

namespace TechSyence.Infrastructure.DataAccess;

public class UnitOfWork(
    TechSyenceDbContext dbContext
    ) : IUnitOfWork
{
    private readonly TechSyenceDbContext _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
