using Bogus;
using TechSyence.Communication;

namespace CommonTestUtilities.Requests;

public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build()
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(user => user.Email, f => f.Internet.Email())
            .RuleFor(user => user.Phone, () => "11912345678")
            .RuleFor(user => user.FirstName, f => f.Name.FirstName())
            .RuleFor(user => user.LastName, f => f.Name.LastName())
            .RuleFor(user => user.Password, f => f.Internet.Password());
    }
}
