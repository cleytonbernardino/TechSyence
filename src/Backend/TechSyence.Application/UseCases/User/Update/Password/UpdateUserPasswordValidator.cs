using FluentValidation;
using TechSyence.Application.SharedValidators;
using TechSyence.Communication;

namespace TechSyence.Application.UseCases.User.Update.Password;

public class UpdateUserPasswordValidator : AbstractValidator<RequestUpdateUserPassword>
{
    public UpdateUserPasswordValidator()
    {
        RuleFor(req => req.OldPassword).SetValidator(new PasswordValidator<RequestUpdateUserPassword>());
        RuleFor(req => req.NewPassword).SetValidator(new PasswordValidator<RequestUpdateUserPassword>());
    }
}
