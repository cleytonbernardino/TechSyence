using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechSyence.Infrastructure.DataAccess;
using Entity = TechSyence.Domain.Entities;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private Entity.User _user = default!;
    private Entity.User _userAdmin = default!;
    private Entity.User _userEmployee = default!;

    public int UserInDataBase { get; private set; } = 3;

    public string UserPassword { get; private set; } = string.Empty;
    
    public string GetEmail() => _user.Email;
    public Guid GetUserIndentifier() => _user.UserIndentifier;

    public void ChangeAdminStatus() => _user = _userAdmin;
    public void ChangeToEmployee() => _user = _userEmployee;

    private void CreateEntities(TechSyenceDbContext dbContext)
    {
        (_user, UserPassword) = UserBuilder.BuildWithPassword();

        _userAdmin = UserBuilder.Build();
        _userAdmin.Id = _user.Id + 1;
        _userAdmin.IsAdmin = true;


        _userEmployee = UserBuilder.Build();
        _userEmployee.Id = _userAdmin.Id + 1;
        _userEmployee.Role = TechSyence.Domain.Enums.UserRolesEnum.EMPLOYEE;

        dbContext.Users.Add(_user);
        dbContext.Users.Add(_userAdmin);
        dbContext.Users.Add(_userEmployee);
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

                CreateEntities(dbContext);
                dbContext.SaveChanges();
            });
    }
}
