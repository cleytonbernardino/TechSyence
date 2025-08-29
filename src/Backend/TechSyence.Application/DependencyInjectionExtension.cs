using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqids;
using TechSyence.Application.UseCases.Company.List;
using TechSyence.Application.UseCases.Company.ListUsers;
using TechSyence.Application.UseCases.Company.Register;
using TechSyence.Application.UseCases.Login;
using TechSyence.Application.UseCases.User.Register;
using TechSyence.Application.UseCases.User.Update;
using TechSyence.Application.UseCases.WhatsApp.ReciverJson;

namespace TechSyence.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection service, IConfiguration configuration)
    {
        AddUseCase(service);
        AddIdEncoder(service, configuration);
    }

    private static void AddUseCase(IServiceCollection service)
    {
        service.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        service.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        service.AddScoped<IListCompanyUsersUseCase, ListCompanyUsersUseCase>();
        service.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        service.AddScoped<IReciverJsonUseCase, ReciverJsonUseCase>();
        service.AddScoped<IRegisterCompanyUseCase, RegisterCompanyUseCase>();
        service.AddScoped<IListCompaniesUseCase, ListCompaniesUseCase>();
    }

    private static void AddIdEncoder(IServiceCollection services,  IConfiguration configuration)
    {
        SqidsEncoder<long> sqids = new(new SqidsOptions()
        {
            MinLength = 3,
            Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
        });
        services.AddSingleton(sqids);
    }
}
