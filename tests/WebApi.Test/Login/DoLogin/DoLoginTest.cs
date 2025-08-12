using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;
using System.Globalization;
using TechSyence.Communiction.Requests;
using TechSyence.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest(
    CustomWebApplicationFactory factory
    ) : TechSyenceClassFixture(factory)
{

    private const string METHOD = "login";
    
    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = factory.GetEmail(),
            Password = factory.UserPassword
        };

        var response = await DoPostAsync(METHOD, request);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Email_Credentials(string culture)
    {
        var request = new RequestLoginJson
        {
            Email = "tes@email.com",
            Password = factory.UserPassword
        };

        var response = await DoPostAsync(METHOD, request, culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var errorList = await GetArrayFromResponse(response);

        errorList
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Password_Credentials(string culture)
    {
        var request = new RequestLoginJson
        {
            Email = factory.GetEmail(),
            Password = factory.UserPassword + "abc"
        };

        var response = await DoPostAsync(METHOD, request, culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var errorList = await GetArrayFromResponse(response);

        errorList
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Email(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();
        request.Email = "tes";

        var response = await DoPostAsync(METHOD, request, culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errorList = await GetArrayFromResponse(response);

        errorList
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("INVALID_EMAIL", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Password(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();
        request.Password = string.Empty;

        var response = await DoPostAsync(METHOD, request, culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errorList = await GetArrayFromResponse(response);

        errorList
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("PASSWORD_EMPTY", new CultureInfo(culture)));
    }
}
