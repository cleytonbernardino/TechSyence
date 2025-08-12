using CommonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
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
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await DoPostAsync(METHOD, request);

        var rootElement = await GetRootElement(response);

        var firstName = rootElement.GetProperty("firstName").ToString();

        request.FirstName.ShouldBe(firstName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public async Task Success_Without_Last_Name(string? lastName)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.LastName = lastName;

        var response = await DoPostAsync(METHOD, request);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_First_Name_Empty(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.FirstName = string.Empty;

        var response = await DoPostAsync(METHOD, request, culture);

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
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var response = await DoPostAsync(METHOD, request, culture);
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
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = factory.GetEmail();

        var response = await DoPostAsync(METHOD, request, culture);
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
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Phone = string.Empty;

        var response = await DoPostAsync(METHOD, request, culture);
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
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Phone = "(11) 91741824";

        var response = await DoPostAsync(METHOD, request, culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errorList = await GetArrayFromResponse(response);

        errorList
            .ShouldHaveSingleItem().ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("PHONE_NOT_VALID", new CultureInfo(culture)));
    }
}
