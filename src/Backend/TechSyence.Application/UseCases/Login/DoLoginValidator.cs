using FluentValidation;
using TechSyence.Application.SharedValidators;
using TechSyence.Communication;
using TechSyence.Exceptions;

namespace TechSyence.Application.UseCases.Login;

public class DoLoginValidator : AbstractValidator<RequestLogin>
{
    public DoLoginValidator()
    {
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestLogin>());
        When(user => 
            !string.IsNullOrEmpty(user.Email),
            () => RuleFor(u => u.Email).EmailAddress().WithMessage(ResourceMessagesException.INVALID_EMAIL)
        );
    }
}
