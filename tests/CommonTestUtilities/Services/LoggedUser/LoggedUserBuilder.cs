using Moq;
using TechSyence.Domain.Entities;
using TechSyence.Domain.Services.LoggedUser;

namespace CommonTestUtilities.Services.LoggedUser;

public class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var mock = new Mock<ILoggedUser>();
        mock.Setup(user => user.User()).ReturnsAsync(user);
        return mock.Object;
    }
}
