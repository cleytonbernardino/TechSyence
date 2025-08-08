using Bogus;
using CommonTestUtilities.Cryptography;
using TechSyence.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    private static Faker<User> GenerateUser()
    {
        return new Faker<User>()
            .RuleFor(user => user.Id, () => 1)
            .RuleFor(user => user.UpdatedOn, () => DateTime.UtcNow)
            .RuleFor(user => user.UserIndentifier, () => Guid.NewGuid())
            .RuleFor(user => user.Email, f => f.Internet.Email())
            .RuleFor(user => user.Phone, () => "(11) 981628391")
            .RuleFor(user => user.FirstName, f => f.Name.FirstName())
            .RuleFor(user => user.LastName, f => f.Name.LastName());
    }

    public static User Build()
    {
        return GenerateUser()
            .RuleFor(user => user.Password, f => f.Internet.Password());
    }

    public static (User user, string password) BuildWithPassword()
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var password = new Faker().Internet.Password();
        var encriptyPassword = passwordEncripter.Encript(password);

        return (
            user: GenerateUser().RuleFor(user => user.Password, () => encriptyPassword),
            password: password
        );
    }
}
