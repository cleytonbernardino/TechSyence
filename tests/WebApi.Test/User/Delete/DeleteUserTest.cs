using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Globalization;
using System.Net;
using TechSyence.Domain.Enums;
using TechSyence.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Delete;

public class DeleteUserTest(CustomWebApplicationFactory factory) : TechSyenceClassFixture(factory)
{
    private const string METHOD = "user";
    
    private readonly IdEncoderForTests _idEncoder = new();
    
    [Fact]
    public async Task Success()
    {
        var userToDelete = factory.InjectUser(UserBuilder.Build());

        string url = $"{METHOD}/{_idEncoder.Encode(userToDelete.Id)}";
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.ManagerUser.UserIndentifier, UserRolesEnum.MANAGER);
        
        var response = await DoDeleteAsync(url, token);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Permission(string culture)
    {
        string url = $"{METHOD}/{_idEncoder.Encode(factory.AdminUser.Id)}";
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.EmployeeUser.UserIndentifier, UserRolesEnum.MANAGER);
        
        var response = await DoDeleteAsync(url, token, culture: culture);
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
        string url = $"{METHOD}/{_idEncoder.Encode(factory.EmployeeUser.Id)}";
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.ManagerUser.UserIndentifier, UserRolesEnum.EMPLOYEE);
        
        var response = await DoDeleteAsync(url, token);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
