namespace TechSyence.Domain.Security.Token;

public interface IAccessTokenClaims
{
    Guid GetUserIndentifier();
}
