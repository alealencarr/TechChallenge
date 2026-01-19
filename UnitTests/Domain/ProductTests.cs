using Domain.Entities.Aggregates.AggregateProduct;
using FluentAssertions;
using Xunit;

namespace Orders.UnitTests.Domain.Entities
{
    public class ProductTests
    {
        [Fact]
        public void Deve_Criar_Produto_Simples_Com_Sucesso()
        {
            var product = Product.Create("Coca-Cola", 5.00m, Guid.NewGuid(), "Refrigerante", new List<ProductIngredient>(), new List<ProductImage>(), false);


            product.Should().NotBeNull();
            product.Name.Should().Be("Coca-Cola");
            product.IsLanche.Should().BeFalse();
        }

        [Fact]
        public void Deve_Criar_Produto_Lanche_Com_Sucesso()
        {

            var ingredientes = new List<ProductIngredient> { new ProductIngredient(Guid.NewGuid(), 1) };


            var product = Product.Create("X-Salada", 20.00m, Guid.NewGuid(), "Lanche", ingredientes, new List<ProductImage>(), true);


            product.Should().NotBeNull();
            product.IsLanche.Should().BeTrue();
            product.ProductIngredients.Should().HaveCount(1);
        }

        [Theory]
        [InlineData("", "Desc", 10)]
        [InlineData("Nome", "", 10)]
        [InlineData("Nome", "Desc", 0)]
        public void Deve_Lancar_Erro_Validacao_Basica(string nome, string desc, decimal preco)
        {
            Action act = () => Product.Create(nome, preco, Guid.NewGuid(), desc, new List<ProductIngredient>(), new List<ProductImage>(), false);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Deve_Lancar_Erro_Se_Lanche_Sem_Ingrediente()
        {
            Action act = () => Product.Create("Lanche", 10m, Guid.NewGuid(), "Desc", new List<ProductIngredient>(), new List<ProductImage>(), true);

            act.Should().Throw<ArgumentException>()
               .WithMessage("É necessário informar pelo menos um ingrediente para criar um produto do tipo Lanche.");
        }

        [Fact]
        public void Deve_Adicionar_Imagens_Corretamente()
        {
            var images = new List<ProductImage> { new ProductImage(new byte[] { 1 }, "foto.png") };

            var product = Product.Create("Prod", 10m, Guid.NewGuid(), "Desc", new List<ProductIngredient>(), images, false);

            product.ProductImages.Should().HaveCount(1);
            product.ProductImages.First().FileName.Should().Contain("imagem-foto.png");
        }
    }
}