﻿using Bogus;
using CommonTestUtilities.Cryptography;
using TechSyence.Communication;
using TechSyence.Domain.Enums;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserBuilder
{
    public static RequestUpdateUser Build()
    {
        var encoder = new IdEncoderForTests();
        return new Faker<RequestUpdateUser>()
            .RuleFor(req => req.UserIdToUpdate, f => encoder.Encode(f.Random.Int(1, 100)))
            .RuleFor(req => req.LastName, f => f.Person.LastName)
            .RuleFor(req => req.Email, f => f.Person.Email)
            .RuleFor(req => req.Phone, () => "11987424156")
            .RuleFor(req => req.Role, () => (int)UserRolesEnum.MANAGER);
    }
}
