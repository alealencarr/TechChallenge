using Application.Common;
using FluentAssertions;
using System.Reflection;
using Xunit;

namespace Application.UnitTests.Common
{
    [Collection("Sequential")]
    public class UtilsTests
    {
        private void ResetBaseUrl()
        {
            var property = typeof(Utils).GetProperty("BaseUrl", BindingFlags.Public | BindingFlags.Static);
            if (property != null)
            {
                property.SetValue(null, string.Empty);
            }
        }

        public UtilsTests()
        {
            ResetBaseUrl();
        }


        [Theory]
        [InlineData("123.456.789-00", "12345678900")]
        [InlineData("111.222.333-44", "11122233344")]
        [InlineData("000.000.000-00", "00000000000")]
        public void FormataCpfSemPontuacao_DeveRemoverPontosETraços(string input, string expected)
        {
            var result = input.FormataCpfSemPontuacao();

            result.Should().Be(expected);
        }

        [Fact]
        public void FormataCpfSemPontuacao_DeveManterStringSeNaoTiverPontuacao()
        {
            var cpfLimpo = "12345678900";

            var result = cpfLimpo.FormataCpfSemPontuacao();

            result.Should().Be(cpfLimpo);
        }


        [Fact]
        public void Configure_DeveDefinirBaseUrl_RemovendoBarraNoFinal()
        {
            var urlComBarra = "https://meusite.com/";

            Utils.Configure(urlComBarra);

            Utils.BaseUrl.Should().Be("https://meusite.com");
        }

        [Fact]
        public void Configure_DeveLancarArgumentNullException_QuandoUrlNula()
        {
            Action act = () => Utils.Configure(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ToAbsoluteUrl_DeveConcatenarUrlCorretamente()
        {
            Utils.Configure("https://api.com");
            var relativePath = "imagens/produto.png";

            var result = relativePath.ToAbsoluteUrl();

            result.Should().Be("https://api.com/imagens/produto.png");
        }

        [Fact]
        public void ToAbsoluteUrl_DeveTratarBarrasDuplicadas()
        {
            Utils.Configure("https://api.com/");
            var relativePath = "/imagens/foto.jpg";

            var result = relativePath.ToAbsoluteUrl();

            result.Should().Be("https://api.com/imagens/foto.jpg");
        }

        [Fact]
        public void ToAbsoluteUrl_DeveLancarInvalidOperation_SeBaseUrlNaoConfigurada()
        {
            ResetBaseUrl(); 

            var relativePath = "teste.png";

            Action act = () => relativePath.ToAbsoluteUrl();

            act.Should().Throw<InvalidOperationException>()
               .WithMessage("BaseUrl was not configured. Call ApplicationUrls.Configure(baseUrl) at startup.");
        }
    }
}