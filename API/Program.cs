using Api.Extensions;
using Application.Common;
using Infrastructure.Configurations;
using Infrastructure.DbContexts;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(n => n.FullName);
});

var baseUrl = builder.Configuration["ApiUrls:Base"];
Utils.Configure(baseUrl);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var cnnStr = builder.Configuration.GetConnectionString(Configuration.ConnectionString);
builder.Services.AddTransient<DataSeeder>();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(cnnStr, options =>
    {
        options.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        );
    });
});




var fileStorageSettings = new FileStorageSettings();
fileStorageSettings.FileBasePath = builder.Environment.WebRootPath;
builder.Services.AddSingleton(fileStorageSettings);
//builder.Services.AddHealthChecks();  

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


#if DEBUG
app.UseHttpsRedirection();
#endif


app.MapEndpoints();

//app.MapHealthChecks("/health", new HealthCheckOptions
//{
//    // Usa o pacote que instalamos para gerar uma resposta JSON bonita.
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});

//// Endpoint principal da sua aplicação (só para exemplo)
//app.MapGet("/", () => "Minha API .NET está funcionando!");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var seeder = services.GetRequiredService<DataSeeder>();

    var retryCount = 0;
    var maxRetries = 10;
    var delay = TimeSpan.FromSeconds(5);

    while (true)
    {
        try
        {
            await context.Database.MigrateAsync();
            await seeder.Initialize();
            break;
        }
        catch (Exception ex)
        {
            retryCount++;
            Console.WriteLine($"Erro ao aplicar migrations (tentativa {retryCount}): {ex.Message}");

            if (retryCount >= maxRetries)
                throw;

            Thread.Sleep(delay);
        }
    }
}


await app.RunAsync();
