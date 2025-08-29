﻿using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using Shouldly;
using TechSyence.Application.UseCases.User.Update;
using TechSyence.Domain.Enums;
using TechSyence.Exceptions;
using TechSyence.Exceptions.ExceptionsBase;
using Entity = TechSyence.Domain.Entities;

namespace UseCases.Test.User.Update;

public class UpdateUserUseCaseTest
{
    private readonly IdEncoderForTests _idEncoder = new();

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserBuilder.Build();

        var useCase = CreateUseCase(user, request.UserIdToUpdate);
        async Task act() => await useCase.Execute(request);

        await act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Email_In_Use()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserBuilder.Build();

        var useCase = CreateUseCase(user, request.UserIdToUpdate, email: request.Email);
        async Task act() => await useCase.Execute(request);

        var errors = await act().ShouldThrowAsync<ErrorOnValidationException>();
        errors.ErrorMessages
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.EMAIL_IN_USE);
    }

    [Fact]
    public async Task Error_User_Without_Permission()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.EMPLOYEE;

        var request = RequestUpdateUserBuilder.Build();

        var userCase = CreateUseCase(user, request.UserIdToUpdate);
        async Task act() => await userCase.Execute(request);

        var errors = await act().ShouldThrowAsync<NoPermission>();
        errors.Message.ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }

    [Fact]
    public async Task Error_Update_Role_Without_Permission()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.RH;

        var request = RequestUpdateUserBuilder.Build();
        request.Role = (int)UserRolesEnum.MANAGER;

        var userCase = CreateUseCase(user, request.UserIdToUpdate);
        async Task act() => await userCase.Execute(request);

        var errors = await act().ShouldThrowAsync<NoPermission>();
        errors.Message.ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }

    private UpdateUserUseCase CreateUseCase(Entity.User user, string encodedRequestId, string? email = null)
    {
        var idEncoder = new IdEncoderBuilder().Decode();
        var loggedUser = LoggedUserBuilder.Build(user);
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(_idEncoder.Decode(encodedRequestId), user);
        var unitOfWork = UnitOfWorkBuilder.Build();

        if (email is not null)
            readOnlyRepository.ExistActiveUserWithEmail(email);

        return new UpdateUserUseCase(
            idEncoder.Build(), loggedUser, readOnlyRepository.Build(), updateOnlyRepository.Build(), unitOfWork
            );
    }
}
