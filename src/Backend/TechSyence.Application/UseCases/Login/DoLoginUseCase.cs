using TechSyence.Communication.Requests;
using TechSyence.Communication.Responses;
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

    public async Task<ResponseResgisteredUserJson> Execute(RequestLoginJson request)
    {
        Validator(request);

        var user = await _repository.GetUserByEmailAndPassword(request.Email, request.Password)
            ?? throw new InvalidLoginException();

        return new ResponseResgisteredUserJson
        {
            FirstName = user.FirstName,
            Tokens = new ResponseTokenJson
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIndentifier),
                RefreshToken = string.Empty
            }
        };
    }

    private static void Validator(RequestLoginJson request)
    {
        DoLoginValidator validator = new();

        var result = validator.Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(
                result.Errors.Select(err => err.ErrorMessage).ToArray());
    }
}
