using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Tokens;
using Shouldly;
using TechSyence.Application.UseCases.Login;
using TechSyence.Communiction.Requests;
using TechSyence.Exceptions.ExceptionsBase;
using Entity = TechSyence.Domain.Entities;

namespace UseCases.Test.Login;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var request = new RequestLoginJson
        {
            Email = user.Email,
            Password = user.Password,
        };

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        await act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Invalid_Email_Credentials()
    {
        var user = UserBuilder.Build();

        var request = new RequestLoginJson
        {
            Email = "tes@gmail.com",
            Password = user.Password,
        };

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        await act().ShouldThrowAsync<InvalidLoginException>();
    }

    [Fact]
    public async Task Error_Invalid_Password_Credentials()
    {
        var user = UserBuilder.Build();

        var request = new RequestLoginJson
        {
            Email = user.Email,
            Password = user.Password + "123",
        };

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        await act().ShouldThrowAsync<InvalidLoginException>();
    }

    [Fact]
    public async Task Error_Invalid_Email()
    {
        var user = UserBuilder.Build();

        var request = new RequestLoginJson
        {
            Email = "Fake",
            Password = user.Password,
        };

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        await act().ShouldThrowAsync<ErrorOnValidationException>();
    }

    [Fact]
    public async Task Error_Invalid_Password()
    {
        var user = UserBuilder.Build();

        var request = new RequestLoginJson
        {
            Email = user.Email,
            Password = string.Empty,
        };

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        await act().ShouldThrowAsync<ErrorOnValidationException>();
    }

    private static DoLoginUseCase CreateUseCase(Entity.User user)
    {
        var repository = new UserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        repository.GetUserByEmailAndPassword(user);

        return new DoLoginUseCase(repository.Build(), accessTokenGenerator);
    }

}
