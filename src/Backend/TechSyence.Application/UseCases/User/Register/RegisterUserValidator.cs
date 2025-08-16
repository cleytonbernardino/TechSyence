using FluentValidation;
using TechSyence.Application.SharedValidators;
using TechSyence.Communication.Requests;
using TechSyence.Exceptions;

namespace TechSyence.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
        RuleFor(user => user.Phone).NotEmpty().WithMessage(ResourceMessagesException.PHONE_EMPTY);
        RuleFor(user => user.FirstName).NotEmpty().WithMessage(ResourceMessagesException.FIRST_NAME_EMPTY);
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.INVALID_EMAIL);
        });
        When(user => !string.IsNullOrEmpty(user.Phone), () =>
            RuleFor(user => user.Phone).Matches(@"^\(?\d{2}\)?\s?9\d{4}-?\d{4}$").WithMessage(ResourceMessagesException.PHONE_NOT_VALID)
        );
    }
}
