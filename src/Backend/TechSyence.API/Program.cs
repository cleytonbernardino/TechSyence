using Microsoft.OpenApi.Models;
using Serilog;
using TechSyence.API.Filter;
using TechSyence.API.Middleware;
using TechSyence.API.Token;
using TechSyence.Application;
using TechSyence.Domain.Security.Token;
using TechSyence.Infrastructure;
using TechSyence.Infrastructure.Extensions;
using TechSyence.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);


// Configure serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Host.UseSerilog((context, service, configuration) =>
{
    configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(service);

});

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(gen =>
{
    gen.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TechSyence API",
        Version = "v1",
        Description = "Some"
    });
});

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
    app.UseSwaggerUI(ui =>
    {
        ui.SwaggerEndpoint("/swagger/v1/swagger.json", "TechSyence v1");
    });
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