using API;
using API.Extensions;
using Infrastructure;
using Infrastructure.Configurations;
using Serilog;

Log.Logger = LogExtensions.ConfigureLog();

try
{
    Log.Information("Iniciando aplicação...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services
           .AddPresentation(builder.Configuration)
           .AddInfrastructure(builder.Configuration)
           .AddSingleton(new FileStorageSettings
           {
               FileBasePath = builder.Environment.WebRootPath
           })
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
