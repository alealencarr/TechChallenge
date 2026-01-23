using Application.Common;
using Application.Presenter.Products;
using Domain.Entities.Aggregates.AggregateProduct;  
using FluentAssertions;
using Xunit;

namespace Products.UnitTests.Application.Presenter
{
    public class ProductPresenterTests
    {
        private readonly ProductPresenter _presenter;
        private const string DefaultMessage = "Msg Teste";

        public ProductPresenterTests()
        {
            Utils.Configure("https://strgtchungryprod.blob.core.windows.net/imagens/");
            _presenter = new ProductPresenter(DefaultMessage);
        }

        #region Constructor & Basic Results

        [Fact]
        public void Constructor_ComNull_DeveUsarStringVazia()
        {

            var presenter = new ProductPresenter(null);
            var result = presenter.RetornoSucess();

            result.Succeeded.Should().BeTrue();

            result.Messages.Should().Contain(string.Empty);
        }

        [Fact]
        public void RetornoSucess_DeveRetornarSucesso()
        {
            var result = _presenter.RetornoSucess();
            result.Succeeded.Should().BeTrue();
            result.Messages.Should().Contain(DefaultMessage);
        }

        [Fact]
        public void Error_Generico_DeveRetornarFalha()
        {
            var msg = "Erro Genérico";
            var result = _presenter.Error<object>(msg);

            result.Succeeded.Should().BeFalse();
            result.Messages.Should().Contain(msg);
            result.Data.Should().BeNull();
        }

        [Fact]
        public void Error_Simples_DeveRetornarFalha()
        {
            var msg = "Erro Simples";
            var result = _presenter.Error(msg);

            result.Succeeded.Should().BeFalse();
            result.Messages.Should().Contain(msg);
        }

        #endregion

        #region Transform Tests

        [Fact]
        public void TransformObject_DeveMapearProdutoCompleto()
        {

            var prodId = Guid.NewGuid();
            var catId = Guid.NewGuid();


            var ingList = new List<ProductIngredient>
            {
                new ProductIngredient(Guid.NewGuid(), 2)
            };


            var imgList = new List<ProductImage>
            {
                new ProductImage(Guid.NewGuid(), prodId, new byte[]{1}, "foto", "path", "png", "foto.png")
            };


            var product = new Product(prodId, "X-Teste", "Desc", 10m, catId, DateTime.Now,  ingList, imgList, true);


            var result = _presenter.TransformObject(product);


            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(prodId);
            result.Data.Name.Should().Be("X-Teste");
            result.Data.Price.Should().Be(10m);
            result.Data.CategorieId.Should().Be(catId);


            result.Data.Ingredients.Should().HaveCount(1);
            result.Data.Ingredients[0].Quantidade.Should().Be(2);

            result.Data.Images.Should().HaveCount(1);
            result.Data.Images[0].Name.Should().Be("foto");

            result.Data.Images[0].Url.Should().Contain("path");
            result.Data.Images[0].Url.Should().Contain("foto.png");
        }

        [Fact]
        public void TransformObject_DeveMapearProdutoSemListas()
        {


            var prodId = Guid.NewGuid();

            var ingList = new List<ProductIngredient>();
            var imgList = new List<ProductImage>();

 
            var product = new Product(prodId, "Simples", "Desc", 10m, Guid.NewGuid(), DateTime.Now, ingList, imgList, false);


            var result = _presenter.TransformObject(product);


            result.Succeeded.Should().BeTrue();
            result.Data.Ingredients.Should().NotBeNull();
            result.Data.Ingredients.Should().BeEmpty();  

            result.Data.Images.Should().NotBeNull();
            result.Data.Images.Should().BeEmpty();
        }

        [Fact]
        public void TransformList_DeveMapearListaDeProdutos()
        {


            var p1 = new Product(Guid.NewGuid(), "P1", "Desc", 10m, Guid.NewGuid(), DateTime.Now, new List<ProductIngredient>(), new List<ProductImage>(), false);
            var p2 = new Product(Guid.NewGuid(), "P2", "Desc", 10m, Guid.NewGuid(), DateTime.Now, new List<ProductIngredient>(), new List<ProductImage>(), false);


            var lista = new List<Product> { p1, p2 };


            var result = _presenter.TransformList(lista);


            result.Succeeded.Should().BeTrue();
            result.Data.Should().HaveCount(2);
            result.Data[0].Name.Should().Be("P1");
            result.Data[1].Name.Should().Be("P2");
        }

        #endregion
    }
}