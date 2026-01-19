using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Shared.DTO;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Application.Externals;
public class TokenAuthenticationHandler(
    IHttpClientFactory clientFactory,
    IMemoryCache memoryCache,
    IConfiguration configuration)
    : DelegatingHandler
{
    private const string ConfigHeaderName = "X-Client-Config-Key";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!request.Headers.TryGetValues(ConfigHeaderName, out var values))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var clientConfigKey = values.FirstOrDefault();

        request.Headers.Remove(ConfigHeaderName);

        if (string.IsNullOrEmpty(clientConfigKey))
            return await base.SendAsync(request, cancellationToken);

        var cacheKey = $"auth-token-{clientConfigKey}";
        if (!memoryCache.TryGetValue(cacheKey, out string? token))
        {
            token = await GetTokenAsync(clientConfigKey);

            memoryCache.Set(cacheKey, token, TimeSpan.FromMinutes(50));
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string> GetTokenAsync(string clientConfigKey)
    {
        var clientId = configuration[$"HttpClientsAuth:{clientConfigKey}:ClientId"];
        var clientSecret = configuration[$"HttpClientsAuth:{clientConfigKey}:ClientSecret"];

        var authClient = clientFactory.CreateClient("AuthLambdaClient");

        var response = await authClient.PostAsJsonAsync("", new
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        });

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthTokenResponse>();
        return result!.access_token;
    }
}

 