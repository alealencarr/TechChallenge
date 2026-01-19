using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.UseCases.Products;
using FluentAssertions;
using Moq;
using Shared.DTO.Product.Input;
using Xunit;

namespace Products.UnitTests.Application.UseCases
{
    public class GetProductByIdUseCaseTests
    {
        private readonly Mock<IProductDataSource> _dataSourceMock;
        private readonly ProductGateway _gateway;
        private readonly GetProductByIdUseCase _useCase;

        public GetProductByIdUseCaseTests()
        {
            _dataSourceMock = new Mock<IProductDataSource>();
            _gateway = ProductGateway.Create(_dataSourceMock.Object);
            _useCase = GetProductByIdUseCase.Create(_gateway);
        }

        [Fact]
        public async Task Run_DeveRetornarProduto_QuandoIdExiste()
        {
            var id = Guid.NewGuid();
            var dto = new ProductInputDto(id, DateTime.Now, "Coca-Cola", "Refrigerante", 5m, Guid.NewGuid(), new List<ProductImageInputDto>(),
                new List<ProductIngredientInputDto>(), false);

            _dataSourceMock.Setup(x => x.GetById(id))
                           .ReturnsAsync(dto);

            var result = await _useCase.Run(id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.Name.Should().Be("Coca-Cola");
        }

        [Fact]
        public async Task Run_DeveRetornarNulo_QuandoIdNaoExiste()
        {
            var id = Guid.NewGuid();

            _dataSourceMock.Setup(x => x.GetById(id))
                           .ReturnsAsync((ProductInputDto?)null);

            var result = await _useCase.Run(id);

            result.Should().BeNull();
        }
    }
}