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

    public string UserPassword { get; private set; } = string.Empty;
    public string GetEmail() => _user.Email;

    private void StartDatabase(TechSyenceDbContext dbContext)
    {
        (_user, UserPassword) = UserBuilder.BuildWithPassword();

        dbContext.Users.Add(_user);
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

                StartDatabase(dbContext);
            });
    }
}
