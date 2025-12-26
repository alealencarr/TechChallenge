using API;
using API.Extensions;
using Application.Interfaces.Services;
using Infrastructure;
using Infrastructure.Services;
using Serilog;

Log.Logger = LogExtensions.ConfigureLog();

try
{
    Log.Information("Iniciando aplicação...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services
           .AddPresentation(builder.Configuration)
           .AddInfrastructure(builder.Configuration)
           .AddHealthChecks().AddHealthApi().AddHealthDb(builder.Configuration);

    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<ITokenService, TokenService>();

    var app = builder.Build();

    await app.InitializeApp(Log.Logger);

    app.RegisterPipeline();
    app.AddHealthChecks();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplica��o terminou inesperadamente");
}
finally
{
    Log.CloseAndFlush();
}
