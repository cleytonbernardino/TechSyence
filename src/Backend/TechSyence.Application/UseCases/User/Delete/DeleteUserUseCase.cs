﻿using TechSyence.Domain.Enums;
using TechSyence.Domain.Repositories;
using TechSyence.Domain.Repositories.User;
using TechSyence.Domain.Services.LoggedUser;
using TechSyence.Exceptions;
using TechSyence.Exceptions.ExceptionsBase;
using Entity = TechSyence.Domain.Entities;

namespace TechSyence.Application.UseCases.User.Delete;

public class DeleteUserUseCase(
    ILoggedUser loggedUser,
    IUserUpdateOnlyRepository updateOnlyRepository,
    IUnitOfWork unitOfWork
    ) : IDeleteUserUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserUpdateOnlyRepository _repository = updateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task Execute(long userToDeleteId)
    {
        var loggedUser = await _loggedUser.User();

        var user = await _repository.GetById(userToDeleteId, loggedUser.CompanyId);
        if (user is null)
            throw new NotFoundException(ResourceMessagesException.USER_NOT_FOUND);
        
        CanDeleteUser(loggedUser, user);

        user.Active = false;
        user.UpdatedOn = DateTime.UtcNow;

        _repository.UpdateUser(user);
        await _unitOfWork.Commit();
    }

    private static void CanDeleteUser(Entity.User user, Entity.User userToBeDeleted)
    {
        if (user.IsAdmin || user.Role == UserRolesEnum.MANAGER)
            return;
        
        List<UserRolesEnum> rolesAllowed = new()
        {
            UserRolesEnum.RH, UserRolesEnum.SUB_MANAGER, UserRolesEnum.MANAGER
        };
        
        if(rolesAllowed.Contains(user.Role) && (int)user.Role > (int)userToBeDeleted.Role)
            return;

        throw new NoPermissionException();
    }
}
