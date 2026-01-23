using Domain.Entities.Aggregates.AggregateProduct;
using FluentAssertions;
using Xunit;

namespace Products.UnitTests.Domain.Entities.Aggregates.AggregateProduct
{
    public class ProductImageTests
    {
        [Fact]
        public void Constructor_BlobAndName_DeveCriarInstancia_QuandoDadosValidos()
        {

            var blob = new byte[] { 1, 2, 3 };
            var name = "foto-teste";
            var expectedBase64 = Convert.ToBase64String(blob);


            var productImage = new ProductImage(blob, name);


            productImage.Id.Should().NotBeEmpty();
            productImage.Blob.Should().BeEquivalentTo(blob);
            productImage.Name.Should().Be(name);


            productImage.MimeType.Should().Be($"data:image/png;base64,{expectedBase64}");


            productImage.FileName.Should().StartWith($"imagem-{name}-");
            productImage.FileName.Should().EndWith(".png");
            productImage.FileName.Should().Contain(productImage.Id.ToString());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Constructor_BlobAndName_DeveLancarErro_QuandoNomeInvalido(string nomeInvalido)
        {

            var blob = new byte[] { 1 };


            Action act = () => new ProductImage(blob, nomeInvalido);


            act.Should().Throw<ArgumentException>()
                .WithMessage("É necessário informar um nome para a imagem do produto.");
        }

        [Fact]
        public void Constructor_FullNewId_DeveGerarIdAutomaticamente()
        {

            var productId = Guid.NewGuid();
            var blob = new byte[] { 10, 20 };
            var name = "foto.png";
            var path = "/storage/foto.png";
            var mime = "image/png";
            var fileName = "arquivo_final.png";


            var image = new ProductImage(productId, blob, name, path, mime, fileName);


            image.Id.Should().NotBeEmpty();
            image.ProductId.Should().Be(productId);
            image.Blob.Should().BeEquivalentTo(blob);
            image.Name.Should().Be(name);
            image.ImagePath.Should().Be(path);
            image.MimeType.Should().Be(mime);
            image.FileName.Should().Be(fileName);
        }

        [Fact]
        public void Constructor_FullWithId_DeveUsarIdFornecido()
        {

            var fixedId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var blob = new byte[] { 255 };


            var image = new ProductImage(fixedId, productId, blob, "nome", "path", "mime", "file");


            image.Id.Should().Be(fixedId);
            image.ProductId.Should().Be(productId);
            image.Blob.Should().BeEquivalentTo(blob);
        }
    }
}