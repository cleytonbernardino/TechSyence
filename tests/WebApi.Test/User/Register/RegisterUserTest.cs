using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Globalization;
using System.Net;
using TechSyence.Domain.Enums;
using TechSyence.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;

public class RegisterUserTest(
    CustomWebApplicationFactory factory
    ) : TechSyenceClassFixture(factory)
{
    private const string METHOD = "user";

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.GetUserIndentifier(), UserRolesEnum.MANAGER);
        
        var request = RequestRegisterUseBuilder.Build();

        var response = await DoPostAsync(METHOD, request, token: token);

        var rootElement = await GetRootElement(response);

        var firstName = rootElement.GetProperty("firstName").ToString();

        request.FirstName.ShouldBe(firstName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public async Task Success_Without_Last_Name(string lastName)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.GetUserIndentifier(), UserRolesEnum.MANAGER);

        var request = RequestRegisterUseBuilder.Build();
        request.LastName = lastName;

        var response = await DoPostAsync(METHOD, request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_First_Name_Empty(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.GetUserIndentifier(), UserRolesEnum.MANAGER);

        var request = RequestRegisterUseBuilder.Build();
        request.FirstName = string.Empty;

        var response = await DoPostAsync(method: METHOD, request: request, token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);


        var errorList = await GetArrayFromResponse(response);
        errorList
            .ShouldHaveSingleItem().ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("FIRST_NAME_EMPTY", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Email_Empty(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.GetUserIndentifier(), UserRolesEnum.MANAGER);

        var request = RequestRegisterUseBuilder.Build();
        request.Email = string.Empty;

        var response = await DoPostAsync(method: METHOD, request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errorList = await GetArrayFromResponse(response);

        errorList
            .ShouldHaveSingleItem().ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("EMAIL_EMPTY", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Email_In_Use(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.GetUserIndentifier(), UserRolesEnum.MANAGER);

        var request = RequestRegisterUseBuilder.Build();
        request.Email = factory.GetEmail();

        var response = await DoPostAsync(method: METHOD, request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errorList = await GetArrayFromResponse(response);

        errorList
            .ShouldHaveSingleItem().ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("EMAIL_IN_USE", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Phone_Empty(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.GetUserIndentifier(), UserRolesEnum.MANAGER);

        var request = RequestRegisterUseBuilder.Build();
        request.Phone = string.Empty;

        var response = await DoPostAsync(method: METHOD, request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errorList = await GetArrayFromResponse(response);

        errorList
            .ShouldHaveSingleItem().ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("PHONE_EMPTY", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Phone_Is_Returning_Error_With_Incorrect_Patterns(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.GetUserIndentifier(), UserRolesEnum.MANAGER);

        var request = RequestRegisterUseBuilder.Build();
        request.Phone = "(11) 91741824";

        var response = await DoPostAsync(method: METHOD, request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errorList = await GetArrayFromResponse(response);

        errorList
            .ShouldHaveSingleItem().ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("PHONE_NOT_VALID", new CultureInfo(culture)));
    }
}
