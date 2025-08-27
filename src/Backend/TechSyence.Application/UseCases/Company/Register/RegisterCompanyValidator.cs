﻿using FluentValidation;
using TechSyence.Communication;
using TechSyence.Exceptions;

namespace TechSyence.Application.UseCases.Company.Register;
public class RegisterCompanyValidator : AbstractValidator<RequestRegisterCompany>
{
    public RegisterCompanyValidator()
    {
        RuleFor(req => req.CNJP).NotEmpty().WithMessage(ResourceMessagesException.CNPJ_EMPTY);
        RuleFor(req => req.LegalName).NotEmpty().WithMessage(ResourceMessagesException.LEGAL_NAME_EMPTY).MaximumLength(100);
        RuleFor(req => req.DoingBusinessAs).MaximumLength(100).WithMessage(ResourceMessagesException.DBA_MAX_LENGTH);
        RuleFor(req => req.CEP).NotEmpty().WithMessage(ResourceMessagesException.CEP_EMPTY);
        RuleFor(req => req.AddressNumber).NotEmpty().WithMessage(ResourceMessagesException.ADDRESS_NUMBER_EMPTY);
        RuleFor(req => req.PhoneNumber).NotEmpty().WithMessage(ResourceMessagesException.PHONE_EMPTY);
        When(req => !string.IsNullOrWhiteSpace(req.BusinessEmail), () =>
            RuleFor(request => request.BusinessEmail).EmailAddress().WithMessage(ResourceMessagesException.INVALID_EMAIL)
        );
        When(req => !string.IsNullOrWhiteSpace(req.PhoneNumber), () =>
           RuleFor(request => request.PhoneNumber).Matches(@"^\(?\d{2}\)?\s?9\d{4}-?\d{4}$").WithMessage(ResourceMessagesException.PHONE_NOT_VALID)
        );
        When(req => !string.IsNullOrWhiteSpace(req.CNJP), () =>
            RuleFor(request => request.CNJP).Matches(@"^\d{2}\.?\d{3}\.?\d{3}/?\d{4}-?\d{2}$").WithMessage(ResourceMessagesException.CNPJ_INVALID)
        );
        When(req => !string.IsNullOrWhiteSpace(req.CEP), () =>
            RuleFor(request => request.CEP).Matches(@"^\d{5}-?\d{3}$").WithMessage(ResourceMessagesException.CEP_INVALID)
        );
    }
}
