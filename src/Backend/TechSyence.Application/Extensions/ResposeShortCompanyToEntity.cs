using TechSyence.Communication;
using TechSyence.Domain.Entities;

namespace TechSyence.Application.Extensions;

internal static class ResposeShortCompanyToEntity
{
    public static ShortCompany ToShortCompany(this ResponseShortCompany response)
    {
        return new ShortCompany
        {
            DoingBusinessAs = response.DoingBusinessAs,
            SubscriptionStatus = response.SubscriptionStatus,
        };
    }

    public static ResponseShortCompany ToResponse(this ShortCompany company)
    {
        return new ResponseShortCompany
        {
             DoingBusinessAs = company.DoingBusinessAs,
             SubscriptionStatus = company.SubscriptionStatus,
        };
    }
}
