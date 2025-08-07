using Microsoft.EntityFrameworkCore;
using TechSyence.Domain.Entities;
using TechSyence.Domain.Repositories.User;

namespace TechSyence.Infrastructure.DataAccess.Repositories;

internal class UserRepository(
    TechSyenceDbContext dbContext
    ) : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly TechSyenceDbContext _dbContext = dbContext;

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext
            .Users
            .AnyAsync(user => user.Email == email && user.Active == true);
    }

    public async Task RegisterUser(User user) => await _dbContext.Users.AddAsync(user);
}
