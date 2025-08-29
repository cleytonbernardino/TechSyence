using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechSyence.Domain.Enums;
using TechSyence.Infrastructure.DataAccess;
using Entity = TechSyence.Domain.Entities;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Entity.User EmployeeUser { get; private set; } 
    public Entity.User RHUser { get; private set; } 
    public Entity.User SubManagerUser { get; private set; } 
    public Entity.User ManagerUser { get; private set; }
    public Entity.User AdminUser { get; private set; }

    public int UserInDataBase { get; private set; } = 1;

    public string UserPassword { get; private set; } = string.Empty;

    public Entity.User InjectUser(Entity.User user)
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TechSyenceDbContext>();

        user.Id = ++UserInDataBase;
        dbContext.Add(user);
        dbContext.SaveChanges();
        return user;
    }

    private void RegisterInitialUsers(TechSyenceDbContext dbContext)
    {
        (ManagerUser, UserPassword) = UserBuilder.BuildWithPassword();

        EmployeeUser = UserBuilder.Build();
        EmployeeUser.Id = ++UserInDataBase;
        EmployeeUser.Role = UserRolesEnum.EMPLOYEE;
        
        RHUser = UserBuilder.Build();
        RHUser.Id = ++UserInDataBase;
        RHUser.Role = UserRolesEnum.RH;
        
        SubManagerUser = UserBuilder.Build();
        SubManagerUser.Id = ++UserInDataBase;
        SubManagerUser.Role = UserRolesEnum.SUB_MANAGER;
        
        AdminUser = UserBuilder.Build();
        AdminUser.Id = ++UserInDataBase;
        AdminUser.IsAdmin = true;

        dbContext.Add(EmployeeUser);
        dbContext.Add(RHUser);
        dbContext.Add(SubManagerUser);
        dbContext.Add(ManagerUser);
        dbContext.Add(AdminUser);

        dbContext.SaveChanges();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TechSyenceDbContext>));
                if (descriptor is not null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<TechSyenceDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<TechSyenceDbContext>();
                dbContext.Database.EnsureDeleted();

                RegisterInitialUsers(dbContext);
            });
    }
}
