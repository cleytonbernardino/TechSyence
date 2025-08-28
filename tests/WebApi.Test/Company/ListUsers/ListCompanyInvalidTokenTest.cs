using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;
using TechSyence.Domain.Enums;
using WebApi.Test.InlineData;

namespace WebApi.Test.Company.ListUsers;

public class ListCompanyInvalidTokenTest(CustomWebApplicationFactory factory) : TechSyenceClassFixture(factory)
{
    private const string METHOD = "company/users";

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Token(string culture)
    {
        var response = await DoGetAsync(METHOD, token: "TokenInvalid", culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Token(string culture)
    {
        var response = await DoGetAsync(METHOD,token: "", culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Without_User(string culture)
    {
        string token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid(), UserRolesEnum.MANAGER);

        var response = await DoGetAsync(METHOD, token, culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
