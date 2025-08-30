using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services.LoggedUser;
using Shouldly;
using TechSyence.Application.UseCases.User.Delete;
using TechSyence.Communication;
using TechSyence.Domain.Enums;
using TechSyence.Exceptions.ExceptionsBase;
using Entity = TechSyence.Domain.Entities;

namespace UseCases.Test.User.Delete;

public class DeleteUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        
        var useCase = CreateUseCase(user, 1);
        async Task act() => await useCase.Execute(1);

        await act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_User_Without_Permission()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.EMPLOYEE;

        var useCase = CreateUseCase(user, 1);
        async Task act() => await useCase.Execute(1);

        var errors = await act().ShouldThrowAsync<NoPermissionException>();
        errors.Message.ShouldBe(TechSyence.Exceptions.ResourceMessagesException.NO_PERMISSION);
    }
    
    [Fact]
    public async Task Error_User_Not_Found()
    {
        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user, 1);
        async Task act() => await useCase.Execute(2);

        var errors = await act().ShouldThrowAsync<NotFoundException>();
        errors.Message.ShouldBe(TechSyence.Exceptions.ResourceMessagesException.USER_NOT_FOUND);
    }
    
    private DeleteUserUseCase CreateUseCase(Entity.User user, long userToDeleteId)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new UserUpdateOnlyRepositoryBuilder().GetById(userToDeleteId, user);
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new DeleteUserUseCase(loggedUser, repository.Build(), unitOfWork);
    }
}
