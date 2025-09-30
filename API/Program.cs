using API;
using API.Extensions;
using Application.Interfaces.Services;
using Infrastructure;
using Infrastructure.Configurations;
using Infrastructure.Services;
using Serilog;

Log.Logger = LogExtensions.ConfigureLog();

try
{
    Log.Information("Iniciando aplicação...");

    var builder = WebApplication.CreateBuilder(args);
    
    var fileStorageSettings = new FileStorageSettings();
    builder.Configuration.GetSection("FileStorage").Bind(fileStorageSettings);

    builder.Services
           .AddPresentation(builder.Configuration)
           .AddInfrastructure(builder.Configuration)
           .AddSingleton(fileStorageSettings)
           .AddHealthChecks().AddHealthApi().AddHealthDb(builder.Configuration);

    var app = builder.Build();

    await app.InitializeApp(Log.Logger);

    app.RegisterPipeline();
    app.AddHealthChecks();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplicação terminou inesperadamente");
}
finally
{
    Log.CloseAndFlush();
}
