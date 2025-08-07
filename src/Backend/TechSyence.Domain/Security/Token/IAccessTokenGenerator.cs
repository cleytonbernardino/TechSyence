namespace TechSyence.Domain.Security.Token;

public interface IAccessTokenGenerator
{
    string Generate(Guid userIndentifier);
}
