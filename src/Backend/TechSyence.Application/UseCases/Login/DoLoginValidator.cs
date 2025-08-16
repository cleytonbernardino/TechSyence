using FluentValidation;
using TechSyence.Application.SharedValidators;
using TechSyence.Communication.Requests;
using TechSyence.Exceptions;

namespace TechSyence.Application.UseCases.Login;

public class DoLoginValidator : AbstractValidator<RequestLoginJson>
{
    public DoLoginValidator()
    {
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestLoginJson>());
        When(user => 
            !string.IsNullOrEmpty(user.Email),
            () => RuleFor(u => u.Email).EmailAddress().WithMessage(ResourceMessagesException.INVALID_EMAIL)
        );
    }
}
