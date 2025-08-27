using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using TechSyence.Application.Services.Encoder;
using TechSyence.Application.Services.MessageQueue;
using TechSyence.Domain.Repositories;
using TechSyence.Domain.Repositories.Company;
using TechSyence.Domain.Repositories.User;
using TechSyence.Domain.Security.Cryptography;
using TechSyence.Domain.Security.Token;
using TechSyence.Domain.Services.LoggedUser;
using TechSyence.Infrastructure.DataAccess;
using TechSyence.Infrastructure.DataAccess.Repositories;
using TechSyence.Infrastructure.Extensions;
using TechSyence.Infrastructure.Security.Cryptography;
using TechSyence.Infrastructure.Security.Token.Access.Generate;
using TechSyence.Infrastructure.Services.LoggedUser;
using TechSyence.Infrastructure.Services.MessageQueue;

namespace TechSyence.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
    {
        AddRepositories(service);
        AddEncripters(service, configuration);
        AddJwtToken(service, configuration);
        //AddQueue(service);
        AddLoggedUser(service);

        if (configuration.IsUnitTestEnviroment())
            return;

        AddDbContext(service, configuration);
        AddFluentMigrator(service, configuration);
    }

    private static void AddRepositories(IServiceCollection service)
    {
        service.AddScoped<IUserReadOnlyRepository, UserRepository>();
        service.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        service.AddScoped<ICompanyWriteOnlyRepository, CompanyRepository>();
        service.AddScoped<ICompanyReadOnlyRepository, CompanyRepository>();
        service.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddEncripters(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordEncripter, Argon2Encripter>();
        services.AddScoped<IIdEncoder, SqidsIdEncoder>();
    }

    private static void AddJwtToken(IServiceCollection services, IConfiguration configuration)
    {
        string signingKey = configuration.GetValue<string>("Settings:JWT:SigningKey")!;
        string expirationTimeMinutesRaw = configuration.GetValue<string>("Settings:JWT:ExpiryInMinutes")!;
        string issureKey = configuration.GetValue<string>("Settings:JWT:Issuer")!;

        bool expirationTimeIsUint = uint.TryParse(expirationTimeMinutesRaw, out uint expirationTimeMinutes);
        if (!expirationTimeIsUint)
            expirationTimeMinutes = 5;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signingKey!)),
                ClockSkew = new TimeSpan(0),
                ValidateIssuer = true,
                ValidIssuer = issureKey,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
            });

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey, issureKey));
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

    private static void AddQueue(IServiceCollection services) => services.AddSingleton<IMessageQueue, MessageQueue>();

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();
}
