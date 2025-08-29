﻿using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;
using TechSyence.Domain.Enums;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update;

public class UpdateUserInvalidTokenTest(CustomWebApplicationFactory factory) : TechSyenceClassFixture(factory)
{
    private const string METHOD = "user";

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Token(string culture)
    {
        var request = RequestRegisterCompanyBuilder.Build();

        var response = await DoPutAsync(METHOD, request, token: "TokenInvalid", culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Token(string culture)
    {
        var request = RequestRegisterCompanyBuilder.Build();

        var response = await DoPutAsync(METHOD, request, token: "", culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Without_User(string culture)
    {
        var request = RequestRegisterCompanyBuilder.Build();

        string token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid(), UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token, culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
