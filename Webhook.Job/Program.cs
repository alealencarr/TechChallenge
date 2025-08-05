using Microsoft.EntityFrameworkCore;
using Webhook.Job;
using Webhook.Job.Services;
using Webhook.Job.Services.Interfaces;
using Infrastructure.DbContexts;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


 
// Registra o WebhookSender e o HttpClient da forma correta e otimizada
// Isso vai gerenciar o ciclo de vida do HttpClient para você.
builder.Services.AddHttpClient<IWebhookSender, WebhookSender>();

// O Worker continua sendo registrado como um Hosted Service (Singleton)
builder.Services.AddHostedService<PaymentWebhookWorker>();

var host = builder.Build();
host.Run();
