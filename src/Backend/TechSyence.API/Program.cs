using TechSyence.API.Filter;
using TechSyence.API.Middleware;
using TechSyence.API.Token;
using TechSyence.Application;
using TechSyence.Domain.Security.Token;
using TechSyence.Infrastructure;
using TechSyence.Infrastructure.Extensions;
using TechSyence.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrationDatabase();

await app.RunAsync();


void MigrationDatabase()
{
    if (builder.Configuration.IsUnitTestEnviroment())
        return;

    string connectionString = builder.Configuration.ConnectionString();

    IServiceScope serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DatabaseMigration.Migrate(connectionString, serviceScope.ServiceProvider);
}

// Apenas para o sonar cloud
public partial class Program
{
    protected Program() { }
}