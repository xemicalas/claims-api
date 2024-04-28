using System.Reflection;
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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddDbContext<AuditContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ClaimsRepository>(
    options =>
    {
        var client = new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));
        var database = client.GetDatabase(builder.Configuration["MongoDb:DatabaseName"]);
        options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
    }
);
builder.Services.AddDbContext<CoversRepository>(
    options =>
    {
        var client = new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));
        var database = client.GetDatabase(builder.Configuration["MongoDb:DatabaseName"]);
        options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
    }
);

builder.Services.AddScoped<IAuditerRepository, AuditerRepository>();
builder.Services.AddScoped<IClaimsRepository, ClaimsRepository>();
builder.Services.AddScoped<ICoversRepository, CoversRepository>();

builder.Services.AddScoped<ICoversService, CoversService>();
builder.Services.AddScoped<IClaimsService, ClaimsService>();
builder.Services.AddScoped<IAuditerService, AuditerService>();
builder.Services.AddSingleton<IPremiumComputeService, PremiumComputeService>();

builder.Services.AddValidatorsFromAssemblyContaining<ClaimRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CoverRequestValidator>();
builder.Services.AddTransient<IValidator<CreateClaimRequest>, ClaimRequestValidator>();
builder.Services.AddTransient<IValidator<CreateCoverRequest>, CoverRequestValidator>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var contractsXmlPath = Path.Combine(AppContext.BaseDirectory, "Claims.WebApi.xml");
    var controllersXmlFile = Path.Combine(AppContext.BaseDirectory, "Claims.WebApi.Contracts.xml");

    c.IncludeXmlComments(contractsXmlPath, true);
    c.IncludeXmlComments(controllersXmlFile, true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<AuditContext>();
//    context.Database.Migrate();
//}

app.Run();

public partial class Program { }