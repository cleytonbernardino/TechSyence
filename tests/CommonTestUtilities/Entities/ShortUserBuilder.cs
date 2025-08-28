﻿using Bogus;
using TechSyence.Domain.Entities;
using TechSyence.Domain.Enums;

namespace CommonTestUtilities.Entities;

public class ShortUserBuilder
{
    private static ShortUser Build()
    {
        return new Faker<ShortUser>()
            .RuleFor(user => user.Id, f => f.Random.Int(1, 1000))
            .RuleFor(user => user.LastLogin, () => DateTime.UtcNow)
            .RuleFor(user => user.FirstName, f => f.Name.FirstName())
            .RuleFor(user => user.LastName, f => f.Name.LastName())
            .RuleFor(user => user.Role, () => UserRolesEnum.MANAGER);
    }

    public static IList<ShortUser> BuildInBatch(int amount = 5)
    {
        List<ShortUser> users = new();
        for (int i = 0; i < amount; i++)
        {
            users.Add(Build());
        }
        return users;
    }
}
