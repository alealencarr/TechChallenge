using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Webhook.Job.Enum;
using Webhook.Job.Services.Interfaces;

namespace Webhook.Job.Services
{
    public class PaymentWebhookWorker : BackgroundService
    {
        private readonly ILogger<PaymentWebhookWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        
        public PaymentWebhookWorker(ILogger<PaymentWebhookWorker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando Worker de pagamento...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessarPagamentosPendentes(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro inesperado no ciclo principal do Worker.");
                }

                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            }
        }

        private async Task ProcessarPagamentosPendentes(CancellationToken stoppingToken)
        {            
            using var scope = _scopeFactory.CreateScope();
            
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var webhookSender = scope.ServiceProvider.GetRequiredService<IWebhookSender>();
            var _config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
           
            var totalPagamentos = await dbContext.Payment.CountAsync(stoppingToken);         

            var pagamentosPendentes = await dbContext.Payment
                .Where(p => !p.Processed)
                .OrderBy(p => p.CreatedAt)
                .Take(10)
                .ToListAsync(stoppingToken);

            if (!pagamentosPendentes.Any())
            {
                return;  
            }

            _logger.LogInformation($"Encontrados {pagamentosPendentes.Count} pagamentos pendentes para processar.");

                     
            foreach (var pagamento in pagamentosPendentes)
            {
                var statusSimulado = (totalPagamentos % 3 == 0) ? EPaymentStatus.Failed : EPaymentStatus.Paid;
                
                var payload = new
                {
                    pagamento.Id,
                    pagamento.OrderId,
                    Status = (int)statusSimulado,
                    pagamento.Amount
                };
                var eventType = $"payment.{statusSimulado.ToString().ToLower()}";

                
                var paymentUrl = _config["ApiUrls:Payment"];
                await webhookSender.SendAsync(payload, paymentUrl, eventType);

                pagamento.Processed = true;                  
            }
            
            await dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}
