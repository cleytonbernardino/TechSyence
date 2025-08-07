namespace TechSyence.Domain.Security.Token;

public interface IAccessTokenValidator
{
    Guid ValidateAndGetUserIdentifier(string token);
}
