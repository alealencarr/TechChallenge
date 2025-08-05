namespace Webhook.Job.Services.Interfaces
{
    public interface IWebhookSender
    {
        Task SendAsync<T>(T payload, string targetUrl, string eventType);
    }
}
