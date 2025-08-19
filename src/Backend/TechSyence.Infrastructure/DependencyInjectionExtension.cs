using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TechSyence.Application.Services.MessageQueue;
using TechSyence.Domain.Repositories;
using TechSyence.Domain.Repositories.User;
using TechSyence.Domain.Security.Cryptography;
using TechSyence.Domain.Security.Token;
using TechSyence.Infrastructure.DataAccess;
using TechSyence.Infrastructure.DataAccess.Repositories;
using TechSyence.Infrastructure.Extensions;
using TechSyence.Infrastructure.Security.Cryptography;
using TechSyence.Infrastructure.Security.Token.Access.Generate;
using TechSyence.Infrastructure.Security.Token.Access.Validator;
using TechSyence.Infrastructure.Services.MessageQueue;

namespace TechSyence.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
    {
        AddRepositories(service);
        AddEncripter(service, configuration);
        AddJwtToken(service, configuration);
        AddQueue(service);

        if (configuration.IsUnitTestEnviroment())
            return;

        AddDbContext(service, configuration);
        AddFluentMigrator(service, configuration);
    }

    private static void AddRepositories(IServiceCollection service)
    {
        service.AddScoped<IUserReadOnlyRepository, UserRepository>();
        service.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        service.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddEncripter(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordEncripter, Argon2Encripter>();
    }

    private static void AddJwtToken(IServiceCollection services, IConfiguration configuration)
    {
        string signingKey = configuration.GetSection("Settings:JWT:SigningKey").Value!;
        string expirationTimeMinutesRaw = configuration.GetSection("Settings:JWT:ExpirationTimeMinutes").Value!;

        bool expirationTimeIsUint = uint.TryParse(expirationTimeMinutesRaw, out uint expirationTimeMinutes);
        if (!expirationTimeIsUint)
            expirationTimeMinutes = 5;

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey));
        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey));
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.ConnectionString();

        services.AddDbContext<TechSyenceDbContext>(option =>
           option.UseMySQL(connectionString)
       );
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
            options
            .AddMySql5()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(Assembly.Load("TechSyence.Infrastructure")).For.All()
        );
    }

    private static void AddQueue(IServiceCollection services)
    {
        services.AddSingleton<IMessageQueue, MessageQueue>();
    }
}
