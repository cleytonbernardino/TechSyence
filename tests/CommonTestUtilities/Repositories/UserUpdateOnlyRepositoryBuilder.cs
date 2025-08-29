using Moq;
using TechSyence.Domain.Repositories.User;
using Entity = TechSyence.Domain.Entities;

namespace CommonTestUtilities.Repositories;

public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUserUpdateOnlyRepository> _mock = new();

    public IUserUpdateOnlyRepository Build() => _mock.Object;

    public UserUpdateOnlyRepositoryBuilder GetById(long userIdToMock, Entity.User mockUserReturn)
    {
        _mock.Setup(rep => rep.GetById(userIdToMock, mockUserReturn.CompanyId)).ReturnsAsync(mockUserReturn);
        return this;
    }
}
