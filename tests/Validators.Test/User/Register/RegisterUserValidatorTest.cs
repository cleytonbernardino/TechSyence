using CommonTestUtilities.Requests;
using Shouldly;
using TechSyence.Application.UseCases.User.Register;
using TechSyence.Communication;

namespace Validators.Test.User.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUserJson request = RequestRegisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Success_Without_Name()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUserJson request = RequestRegisterUserJsonBuilder.Build();
        request.LastName = "";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Email_Empty()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUserJson request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
    }

    [Fact]
    public void Error_Email_Incorrect()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUserJson request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "testing";

        var result = validator.Validate(request);

        result.IsValid!.ShouldBeFalse();
    }

    [Fact]
    public void Error_Phone_Incorrect()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUserJson request = RequestRegisterUserJsonBuilder.Build();
        request.Phone = "928194jh142";

        var result = validator.Validate(request);

        result.IsValid!.ShouldBeFalse();
    }

    [Fact]
    public void Error_Phone_Empty()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUserJson request = RequestRegisterUserJsonBuilder.Build();
        request.Phone = string.Empty;

        var result = validator.Validate(request);

        result.IsValid!.ShouldBeFalse();
    }

    [Fact]
    public void Error_First_Name_Empty()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUserJson request = RequestRegisterUserJsonBuilder.Build();
        request.FirstName = string.Empty;

        var result = validator.Validate(request);

        result.IsValid!.ShouldBeFalse();
    }

    [Fact]
    public void Error_Password_Empty()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUserJson request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid!.ShouldBeFalse();
    }
}
