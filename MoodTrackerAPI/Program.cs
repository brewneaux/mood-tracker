using System.Configuration;
using System.Data;
using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.Options;
using MoodTrackerAPI.Configuration;
using MoodTrackerAPI.DatabaseMigrations;
using MoodTrackerAPI.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = builder.Configuration
    .GetSection("POSTGRES")
    .Get<PostgresConnectionConfiguration>();

builder.Services.AddOptions<PostgresConnectionConfiguration>()
    .Bind(builder.Configuration.GetSection("POSTGRES"))
    .ValidateDataAnnotations()
    .ValidateOnStart();



if (config != null && config.EnableMigrations)
{
    Console.WriteLine("Enabling migrations");
    builder.Services.AddFluentMigratorCore()
        .ConfigureRunner(c =>
        {
            c.AddPostgres()
                .WithVersionTable(new CustomVersionTableMetaData())
                .WithGlobalConnectionString(config.ConnectionString)
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations();
                
        }).AddLogging(lb => lb.AddFluentMigratorConsole());;
}

builder.Services.AddTransient<IDbConnection, NpgsqlConnection>(c => new NpgsqlConnection(config?.ConnectionString));

builder.Services.AddTransient<IResponseCalculator, ResponseCalculator>();
builder.Services.AddTransient<IResponseRepository, ResponseRepository>();



var app = builder.Build();

if (app.Services.GetRequiredService<IOptions<PostgresConnectionConfiguration>>().Value.EnableMigrations)
{
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();