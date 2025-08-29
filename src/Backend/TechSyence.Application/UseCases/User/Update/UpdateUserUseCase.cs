using FluentValidation.Results;
using TechSyence.Application.Extensions;
using TechSyence.Application.Services.Encoder;
using TechSyence.Communication;
using TechSyence.Domain.Enums;
using TechSyence.Domain.Repositories;
using TechSyence.Domain.Repositories.User;
using TechSyence.Domain.Services.LoggedUser;
using TechSyence.Exceptions;
using TechSyence.Exceptions.ExceptionsBase;
using Entity = TechSyence.Domain.Entities;

namespace TechSyence.Application.UseCases.User.Update;

public class UpdateUserUseCase(
    IIdEncoder idEncoder,
    ILoggedUser loggedUser,
    IUserReadOnlyRepository readOnlyRepository,
    IUserUpdateOnlyRepository updateOnlyRepository,
    IUnitOfWork unitOfWork
    ) : IUpdateUserUseCase
{
    private readonly IIdEncoder _idEncoder = idEncoder;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository = updateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(RequestUpdateUser request)
    {
        var loggedUser = await _loggedUser.User();

        bool canUpdate = CanUpdateUser(loggedUser, request.Role);
        if (!canUpdate)
            throw new NoPermission();

        long userToUpdateId = _idEncoder.Decode(request.UserIdToUpdate);
        
        var user = await _updateOnlyRepository.GetById(userToUpdateId, loggedUser.CompanyId)
            ?? throw new NotFoundException(ResourceMessagesException.USER_NOT_FOUND);

        await Validate(user, request);

        user = user.Join(request);
        user.UpdatedOn = DateTime.UtcNow;

        _updateOnlyRepository.UpdateUser(user);
        await _unitOfWork.Commit();
    }

    private static bool CanUpdateUser(Entity.User loggedUser, int targetUserRole)
    {
        if (loggedUser.IsAdmin || loggedUser.Role == UserRolesEnum.MANAGER)
            return true;

        return loggedUser.Role > (UserRolesEnum)targetUserRole;
    }

    private async Task Validate(Entity.User userToUpdate, RequestUpdateUser request)
    {
        UpdateUserValidator validator = new();
        var result = await validator.ValidateAsync(request);

        if(userToUpdate.Email != request.Email)
        {
            bool exist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (exist)
                result.Errors.Add(new ValidationFailure { ErrorMessage = ResourceMessagesException.EMAIL_IN_USE });
        }

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(err => err.ErrorMessage).ToArray());
    }
}
