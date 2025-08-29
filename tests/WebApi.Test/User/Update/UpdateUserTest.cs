using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;
using TechSyence.Application.UseCases.User.Update;
using Entity = TechSyence.Domain.Entities;
using TechSyence.Domain.Enums;
using TechSyence.Exceptions;
using TechSyence.Communication;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using System.Globalization;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update;

public class UpdateUserTest(CustomWebApplicationFactory factory) : TechSyenceClassFixture(factory)
{
    private const string METHOD = "User";
    private readonly IdEncoderForTests _idEncoder = new();

    [Fact]
    public async Task Success()
    {
        string userToUpdate = InjectNewUser();

        var request = RequestUpdateUserBuilder.Build();
        request.UserIdToUpdate = userToUpdate;

        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIndentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Role_Invalid(string culture)
    {
        string userToUpdate = InjectNewUser();

        var request = RequestUpdateUserBuilder.Build();
        request.UserIdToUpdate = userToUpdate;
        request.Role = 1000;

        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIndentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("ROLE_INVALID", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Permission(string culture)
    {
        string userToUpdate = InjectNewUser();
        
        var request = RequestUpdateUserBuilder.Build();
        request.UserIdToUpdate = userToUpdate;

        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.EmployeeUser.UserIndentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("NO_PERMISSION", new CultureInfo(culture)));
    }
    
    [Fact]
    public async Task Error_Authorize_Denies_Invalid_Roles()
    {
        var request = RequestUpdateUserBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.EmployeeUser.UserIndentifier, UserRolesEnum.EMPLOYEE);

        var response = await DoPutAsync(METHOD, request, token);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Email_Invalid(string culture)
    {
        string userToUpdate = InjectNewUser();

        var request = RequestUpdateUserBuilder.Build();
        request.UserIdToUpdate = userToUpdate;
        request.Email = "invalid";

        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIndentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("INVALID_EMAIL", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Phone_Invalid(string culture)
    {
        string userToUpdate = InjectNewUser();

        var request = RequestUpdateUserBuilder.Build();
        request.UserIdToUpdate = userToUpdate;
        request.Phone = "ll 97578-27gg";

        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIndentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("PHONE_NOT_VALID", new CultureInfo(culture)));
    }

    private string InjectNewUser()
    {
        var user = UserBuilder.Build();
        factory.InjectUser(user);
        return _idEncoder.Encode(user.Id);
    }
}
