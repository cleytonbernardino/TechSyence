using Microsoft.EntityFrameworkCore;
using TechSyence.Domain.Entities;
using TechSyence.Domain.Repositories.User;
using TechSyence.Infrastructure.Security.Cryptography;

namespace TechSyence.Infrastructure.DataAccess.Repositories;

internal class UserRepository(
    TechSyenceDbContext dbContext
    ) : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly TechSyenceDbContext _dbContext = dbContext;

    public async Task<bool> ExistActiveUserWithIndentifier(Guid userIndentifier)
    {
        return await _dbContext.Users.AnyAsync(user =>
            user.UserIndentifier == userIndentifier && user.Active
        );
    }

    public async Task<bool> ExistActiveUserWithId(long id)
    {
        return await _dbContext
            .Users
            .AnyAsync(user => user.Id == id && user.Active == true);
    }

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext
            .Users
            .AsNoTracking()
            .AnyAsync(user => user.Email == email && user.Active == true);
    }

    public async Task<User?> GetUserByEmailAndPassword(string email, string password)
    {
        var user = await _dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == email && user.Active == true);

        var isValidPassword = ValidPassword(password, user);
        if (isValidPassword)
            return user;
        return null;
    }

    async Task<User?> IUserUpdateOnlyRepository.GetUserByEmailAndPassword(string email, string password)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Email == email && user.Active == true);
        
        var isValidPassword = ValidPassword(password, user);
        if (isValidPassword)
            return user;
        return null;
    }
    
    public async Task RegisterUser(User user) => await _dbContext.Users.AddAsync(user);

    public void UpdateUser(User user) => _dbContext.Users.Update(user);

    async Task<User?> IUserUpdateOnlyRepository.GetById(long id, long comapanyId) => await _dbContext.Users
        .FirstOrDefaultAsync(user => 
        user.Id == id && 
        user.CompanyId ==  comapanyId
        && user.Active);

    private static bool ValidPassword(string password, User? user)
    {
        if (user is null)
            return false;
        
        return new Argon2Encripter().VerifyPassword(password: password, hashedPassword: user.Password);
    }
}
