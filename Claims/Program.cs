using System.Text.Json.Serialization;
using Claims.Repositories;
using Claims.Repositories.Auditing;
using Claims.Repositories.Repositories;
using Claims.Services;
using Claims.WebApi.Contracts;
using Claims.WebApi.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

Configure(app);

app.Run();

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services
        .AddControllers()
        .AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    AddDatabaseContexts(builder);
    AddRepositories(builder.Services);
    AddServices(builder.Services);
    AddValidators(builder.Services);
    AddSwagger(builder.Services);
    AddLogger(builder.Host);
}

void AddDatabaseContexts(WebApplicationBuilder builder)
{
    var client = new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));
    var database = client.GetDatabase(builder.Configuration["MongoDb:DatabaseName"]);

    builder.Services.AddDbContext<AuditContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddDbContext<ClaimsRepository>(options =>
    {
        options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
    });

    builder.Services.AddDbContext<CoversRepository>(options =>
    {
        options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
    });
}

void AddRepositories(IServiceCollection services)
{
    services.AddScoped<IAuditerRepository, AuditerRepository>();
    services.AddScoped<IClaimsRepository, ClaimsRepository>();
    services.AddScoped<ICoversRepository, CoversRepository>();
}

void AddServices(IServiceCollection services)
{
    services.AddScoped<ICoversService, CoversService>();
    services.AddScoped<IClaimsService, ClaimsService>();
    services.AddScoped<IAuditerService, AuditerService>();
    services.AddSingleton<IPremiumComputeService, PremiumComputeService>();
}

void AddValidators(IServiceCollection services)
{
    services.AddTransient<IValidator<CreateClaimRequest>, ClaimRequestValidator>();
    services.AddTransient<IValidator<CreateCoverRequest>, CoverRequestValidator>();
}

void AddSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        var contractsXmlPath = Path.Combine(AppContext.BaseDirectory, "Claims.WebApi.xml");
        var controllersXmlFile = Path.Combine(AppContext.BaseDirectory, "Claims.WebApi.Contracts.xml");

        c.IncludeXmlComments(contractsXmlPath, true);
        c.IncludeXmlComments(controllersXmlFile, true);
    });
}

void AddLogger(IHostBuilder hostBuilder)
{
    hostBuilder.UseSerilog(new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger());
}

void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    MigrateDatabase(app);
}

void MigrateDatabase(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AuditContext>();
        context.Database.Migrate();
    }
}

public partial class Program { }