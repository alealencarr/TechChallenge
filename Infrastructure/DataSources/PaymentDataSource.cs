using Application.Interfaces.DataSources;
using Polly;
using Polly.Wrap;

namespace Infrastructure.DataSources
{
    public class PaymentDataSource : IPaymentDataSource
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncPolicyWrap<HttpResponseMessage> _resiliencePolicy;

        public PaymentDataSource(HttpClient httpClient)
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
            
            _resiliencePolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy);
        }
        public async Task<byte[]> GenerateQrCodeAsync(Guid id, decimal amount)
        {
            var qrCodeString = await GetQrCode(id, amount);

            var relativeUrl = $"v1/create-qr-code/?size=150x150&data={Uri.EscapeDataString(qrCodeString)}";

            var response = await _resiliencePolicy.ExecuteAsync(() => _httpClient.GetAsync(relativeUrl));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }

        private async Task<string> GetQrCode(Guid id, decimal amount)
        {
            var randomCode = $"qrcode-{id}{amount}";
            return await Task.FromResult(randomCode);
        }
    }
}
