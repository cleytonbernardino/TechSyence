using TechSyence.Communication;
using TechSyence.Domain.Repositories.User;
using TechSyence.Domain.Security.Token;
using TechSyence.Exceptions.ExceptionsBase;

namespace TechSyence.Application.UseCases.Login;

public class DoLoginUseCase(
    IUserReadOnlyRepository repository,
    IAccessTokenGenerator accessTokenGenerator
    ) : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository = repository;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;

    public async Task<ResponseResgisteredUser> Execute(RequestLogin request)
    {
        Validator(request);

        var user = await _repository.GetUserByEmailAndPassword(request.Email, request.Password)
            ?? throw new InvalidLoginException();

        return new ResponseResgisteredUser
        {
            FirstName = user.FirstName,
            Tokens = new ResponseToken
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIndentifier, user.Role, user.IsAdmin),
                RefreshToken = string.Empty
            }
        };
    }

    private static void Validator(RequestLogin request)
    {
        DoLoginValidator validator = new();

        var result = validator.Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(
                result.Errors.Select(err => err.ErrorMessage).ToArray());
    }
}
