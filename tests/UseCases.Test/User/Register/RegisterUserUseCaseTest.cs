using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using CommonTestUtilities.Tokens;
using Shouldly;
using TechSyence.Application.UseCases.User.Register;
using TechSyence.Domain.Enums;
using TechSyence.Exceptions;
using TechSyence.Exceptions.ExceptionsBase;
using Entity = TechSyence.Domain.Entities;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestRegisterUseBuilder.Build();

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        await act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_First_Name_Empty()
    {
        var user = UserBuilder.Build();

        var request = RequestRegisterUseBuilder.Build();
        request.FirstName = string.Empty;

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
        exception.ErrorMessages.Single().ShouldBe(ResourceMessagesException.FIRST_NAME_EMPTY);
    }

    [Fact]
    public async Task Error_Email_In_Use()
    {
        var user = UserBuilder.Build();
        var request = RequestRegisterUseBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        async Task act() => await useCase.Execute(request);

        var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
        exception.ErrorMessages.Single().ShouldBe(ResourceMessagesException.EMAIL_IN_USE);
    }

    [Fact]
    public async Task Error_Very_Low_Permission()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.RH;

        var request = RequestRegisterUseBuilder.Build();
        request.Role = (short)UserRolesEnum.MANAGER;

        var useCase = CreateUseCase(user);
        async Task act() => await useCase.Execute(request);

        await act().ShouldThrowAsync<NoPermission>();
    }

    [Fact]
    public async Task Error_Without_Permission()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.EMPLOYEE;

        var request = RequestRegisterUseBuilder.Build();
        request.Role = (short)UserRolesEnum.MANAGER;

        var useCase = CreateUseCase(user);
        async Task act() => await useCase.Execute(request);

        await act().ShouldThrowAsync<NoPermission>();
    }

    private static RegisterUserUseCase CreateUseCase(Entity.User user, string? email = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        UserReadOnlyRepositoryBuilder readOnlyRepository = new();
        var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if (!string.IsNullOrWhiteSpace(email))
            readOnlyRepository.ExistActiveUserWithEmail(email);

        return new RegisterUserUseCase(
            loggedUser, readOnlyRepository.Build(), writeOnlyRepository, passwordEncripter, accessTokenGenerator, unitOfWork
        );
    }
}
