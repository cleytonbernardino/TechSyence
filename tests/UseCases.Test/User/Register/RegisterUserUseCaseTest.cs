using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using TechSyence.Application.UseCases.User.Register;
using TechSyence.Communication.Requests;
using TechSyence.Domain.Repositories;
using TechSyence.Domain.Repositories.User;
using TechSyence.Domain.Security.Cryptography;
using TechSyence.Domain.Security.Token;
using TechSyence.Exceptions;
using TechSyence.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        RequestRegisterUserJson request = RequestRegisterUserJsonBuilder.Build();

        RegisterUserUseCase useCase = CreateUseCase();

        async Task act() => await useCase.Execute(request);

        await act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_First_Name_Empty()
    {
        RequestRegisterUserJson request = RequestRegisterUserJsonBuilder.Build();
        request.FirstName = string.Empty;

        RegisterUserUseCase useCase = CreateUseCase();

        async Task act() => await useCase.Execute(request);

        var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
        exception.ErrorMessages.Single().ShouldBe(ResourceMessagesException.FIRST_NAME_EMPTY);
    }

    [Fact]
    public async Task Error_Email_In_Use()
    {
        RequestRegisterUserJson request = RequestRegisterUserJsonBuilder.Build();

        RegisterUserUseCase useCase = CreateUseCase(request.Email);

        async Task act() => await useCase.Execute(request);

        var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
        exception.ErrorMessages.Single().ShouldBe(ResourceMessagesException.EMAIL_IN_USE);
    }

    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        UserReadOnlyRepositoryBuilder readOnlyRepository = new();
        IUserWriteOnlyRepository writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        IPasswordEncripter passwordEncripter = PasswordEncripterBuilder.Build();
        IAccessTokenGenerator accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();

        if (!string.IsNullOrWhiteSpace(email))
            readOnlyRepository.ExistActiveUserWithEmail(email);

        return new RegisterUserUseCase(readOnlyRepository.Build(), writeOnlyRepository, passwordEncripter, accessTokenGenerator, unitOfWork);
    }
}
