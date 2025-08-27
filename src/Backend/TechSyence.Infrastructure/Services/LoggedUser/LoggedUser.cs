using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TechSyence.Domain.Entities;
using TechSyence.Domain.Security.Token;
using TechSyence.Domain.Services.LoggedUser;
using TechSyence.Infrastructure.DataAccess;

namespace TechSyence.Infrastructure.Services.LoggedUser;

public class LoggedUser(
    TechSyenceDbContext dbContext,
    ITokenProvider tokenProvider
    ) : ILoggedUser
{
    private readonly TechSyenceDbContext _dbContext = dbContext;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<User> User()
    {
        string token = _tokenProvider.Value().Trim();

        JwtSecurityTokenHandler tokenHandle = new();

        var jwtSecurityToken = tokenHandle.ReadJwtToken(token);

        string indentifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        Guid userIndentifier = Guid.Parse(indentifier);

        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.UserIndentifier == userIndentifier && user.Active);
    }
}
