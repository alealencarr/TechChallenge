using Application.Externals;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Shared.DTO;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Application.UnitTests.Externals;

public class TokenAuthenticationHandlerTests
{
    private readonly Mock<IHttpClientFactory> _clientFactoryMock;
    private readonly IMemoryCache _memoryCache; 
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<HttpMessageHandler> _mainRequestInnerHandlerMock;
    private readonly TokenAuthenticationHandler _sut; 

    public TokenAuthenticationHandlerTests()
    {
        _clientFactoryMock = new Mock<IHttpClientFactory>();
        _configurationMock = new Mock<IConfiguration>();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());

        _mainRequestInnerHandlerMock = new Mock<HttpMessageHandler>();
        _mainRequestInnerHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        _sut = new TokenAuthenticationHandler(
            _clientFactoryMock.Object,
            _memoryCache,
            _configurationMock.Object
        )
        {
            InnerHandler = _mainRequestInnerHandlerMock.Object
        };
    }

    [Fact]
    public async Task SendAsync_DeveIgnorar_SeHeaderNaoEstiverPresente()
    {
        var client = new HttpClient(_sut);
        var request = new HttpRequestMessage(HttpMethod.Get, "http://api-destino.com/resource");

        await client.SendAsync(request);

        _mainRequestInnerHandlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req => req.Headers.Authorization == null),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public async Task SendAsync_DeveUsarTokenDoCache_SeEstiverDisponivel()
    {
        var configKey = "Financeiro";
        var cachedToken = "token-em-cache-123";

        _memoryCache.Set($"auth-token-{configKey}", cachedToken);

        var client = new HttpClient(_sut);
        var request = new HttpRequestMessage(HttpMethod.Get, "http://api-destino.com/resource");
        request.Headers.Add("X-Client-Config-Key", configKey);

        await client.SendAsync(request);

        _mainRequestInnerHandlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Headers.Authorization != null &&
                req.Headers.Authorization.Scheme == "Bearer" &&
                req.Headers.Authorization.Parameter == cachedToken),
            ItExpr.IsAny<CancellationToken>()
        );

        _clientFactoryMock.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Never);

        _mainRequestInnerHandlerMock.Protected().Verify(
           "SendAsync",
           Times.Once(),
           ItExpr.Is<HttpRequestMessage>(req => !req.Headers.Contains("X-Client-Config-Key")),
           ItExpr.IsAny<CancellationToken>()
       );
    }

 

    private bool CheckAuthRequest(HttpRequestMessage req, string expectedId, string expectedSecret)
    {
        if (req.Content == null) return false;

        var json = req.Content.ReadAsStringAsync().Result;
        var doc = JsonDocument.Parse(json);

        var sentId = doc.RootElement.GetProperty("ClientId").GetString();
        var sentSecret = doc.RootElement.GetProperty("ClientSecret").GetString();

        return sentId == expectedId && sentSecret == expectedSecret;
    }
}