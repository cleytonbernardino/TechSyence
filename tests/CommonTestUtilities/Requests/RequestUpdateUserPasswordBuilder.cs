using Bogus;
using TechSyence.Communication;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserPasswordBuilder
{
    public static RequestUpdateUserPassword Build()
    {
        return new Faker<RequestUpdateUserPassword>()
            .RuleFor(req => req.OldPassword, f => f.Internet.Password())
            .RuleFor(req => req.NewPassword, f => f.Internet.Password());
    }
}
