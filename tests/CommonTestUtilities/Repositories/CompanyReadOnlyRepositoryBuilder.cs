﻿using CommonTestUtilities.Entities;
using Moq;
using TechSyence.Domain.Repositories.Company;

namespace CommonTestUtilities.Repositories;

public class CompanyReadOnlyRepositoryBuilder
{
    private readonly Mock<ICompanyReadOnlyRepository> _mock = new();

    public ICompanyReadOnlyRepository Build() => _mock.Object;

    public CompanyReadOnlyRepositoryBuilder ListUsers(int amount = 5)
    {
        var users = ShortUserBuilder.BuildInBatch(amount);

        _mock.Setup(rep => rep.ListUsers(0)).Returns(users);
        return this;
    }
}
