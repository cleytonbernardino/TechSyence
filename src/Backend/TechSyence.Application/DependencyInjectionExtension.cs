using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechSyence.Application.UseCases.Login;
using TechSyence.Application.UseCases.User.Register;

namespace TechSyence.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection service, IConfiguration configurantion)
    {
        AddUseCase(service);
    }

    private static void AddUseCase(IServiceCollection service)
    {
        service.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        service.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
    }
}
