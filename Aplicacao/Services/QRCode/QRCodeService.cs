using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;

namespace Aplicacao.Services.QRCode
{
    public class QRCodeService : IQRCodeService
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncPolicyWrap<HttpResponseMessage> _resiliencePolicy;

        public QRCodeService(HttpClient httpClient) 
        {
            _httpClient = httpClient;

            // Retry com backoff exponencial
            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

            // Circuit Breaker
            var circuitBreakerPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromSeconds(30)
                );

            // Timeout
            var timeoutPolicy = Policy
                .TimeoutAsync<HttpResponseMessage>(3); 

            // Combinando as políticas
            _resiliencePolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy);
        }

        public async Task<byte[]> GerarQrCodeAsync(string qrCodeString)
        {
            var url = $"https://api.qrserver.com/v1/create-qr-code/?size=150x150&data={Uri.EscapeDataString(qrCodeString)}";

            var response = await _resiliencePolicy.ExecuteAsync(() => _httpClient.GetAsync(url));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
