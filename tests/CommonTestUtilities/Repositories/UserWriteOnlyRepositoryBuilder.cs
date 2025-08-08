﻿using Moq;
using TechSyence.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        Mock<IUserWriteOnlyRepository> mock = new();
        return mock.Object;
    }
}
