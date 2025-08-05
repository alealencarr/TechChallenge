using Shared.Helpers;
using System.Text;
using System.Text.Json;
using Webhook.Job.Services.Interfaces;

namespace Webhook.Job.Services
{
    public class WebhookSender : IWebhookSender
    {
        private readonly HttpClient _httpClient;

        public WebhookSender(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendAsync<T>(T payload, string targetUrl, string eventType)
        {
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            content.Headers.Add("X-Webhook-Event", eventType);

            var signature = SignatureGenerator.GenerateSignature(json, WebhookSettings.SecretKey);
            content.Headers.Add("X-Signature", signature);

            var response = await _httpClient.PostAsync(targetUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Webhook failed ({response.StatusCode}): {error}");
            }
        }
    }

}
