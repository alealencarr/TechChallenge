using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.UseCases.Products;
using FluentAssertions;
using Moq;
using Shared.DTO.Product.Input;
using Xunit;

namespace Products.UnitTests.Application.UseCases
{
    public class GetProductByCategorieUseCaseTests
    {
        private readonly Mock<IProductDataSource> _dataSourceMock;
        private readonly ProductGateway _gateway;
        private readonly GetProductByCategorieUseCase _useCase;

        public GetProductByCategorieUseCaseTests()
        {
            _dataSourceMock = new Mock<IProductDataSource>();
            _gateway = ProductGateway.Create(_dataSourceMock.Object);
            _useCase = GetProductByCategorieUseCase.Create(_gateway);
        }

        [Fact]
        public async Task Run_DeveRetornarListaDeProdutos_QuandoEncontrado()
        {
            var categoriaId = Guid.NewGuid().ToString();
            var nomeCategoria = "Lanche";

            var listaDtos = new List<ProductInputDto>
            {
                new ProductInputDto(Guid.NewGuid(), DateTime.Now, "X-Burger", "Desc", 20m, Guid.Parse(categoriaId), new List<ProductImageInputDto>(),
                new List<ProductIngredientInputDto>(), true),
                new ProductInputDto(Guid.NewGuid(), DateTime.Now, "X-Salada", "Desc", 22m, Guid.Parse(categoriaId), new List<ProductImageInputDto>(),
                new List<ProductIngredientInputDto>(), true)
            };

            _dataSourceMock.Setup(x => x.GetByCategorie(categoriaId, nomeCategoria))
                           .ReturnsAsync(listaDtos);

            var result = await _useCase.Run(categoriaId, nomeCategoria);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Name.Should().Be("X-Burger");
        }

        [Fact]
        public async Task Run_DeveRetornarListaVazia_QuandoNaoEncontrar()
        {
            _dataSourceMock.Setup(x => x.GetByCategorie(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync(new List<ProductInputDto>());

            var result = await _useCase.Run("999", "Inexistente");

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Run_DeveLancarExcecao_QuandoErroNoGateway()
        {
            _dataSourceMock.Setup(x => x.GetByCategorie(It.IsAny<string>(), It.IsAny<string>()))
                           .ThrowsAsync(new Exception("Erro de conexão"));

            Func<Task> act = async () => await _useCase.Run("1", "Teste");

            await act.Should().ThrowAsync<Exception>()
                     .WithMessage("Error:Erro de conexão");
        }
    }
}