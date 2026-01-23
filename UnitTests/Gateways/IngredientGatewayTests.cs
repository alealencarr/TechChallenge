using Application.Gateways;
using Application.Interfaces.DataSources;
using FluentAssertions;
using Moq;
using Shared.DTO.Ingredient;

namespace Products.UnitTests.Application.Gateways
{
    public class IngredientGatewayTests
    {
        private readonly Mock<IIngredientDataSource> _dataSourceMock;
        private readonly IngredientGateway _gateway;

        public IngredientGatewayTests()
        {
            _dataSourceMock = new Mock<IIngredientDataSource>();
            _gateway = IngredientGateway.Create(_dataSourceMock.Object);
        }

        [Fact]
        public void Create_DeveRetornarInstanciaValida_QuandoDataSourceInformado()
        {

            var instance = IngredientGateway.Create(_dataSourceMock.Object);


            instance.Should().NotBeNull();
            instance.Should().BeOfType<IngredientGateway>();
        }

        [Fact]
        public async Task GetByIds_DeveRetornarListaDeIngredientes_QuandoDataSourceEncontra()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var idsInput = new List<Guid> { id1, id2 };

            var expectedList = new List<IngredientDto>
            {
                new IngredientDto(id1, DateTime.Now, "Queijo", 1.50m),
                new IngredientDto(id2, DateTime.Now, "Bacon", 3.00m)
            };

            _dataSourceMock.Setup(x => x.GetByIds(idsInput))
                .ReturnsAsync(expectedList);


            var result = await _gateway.GetByIds(idsInput);


            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedList);

            _dataSourceMock.Verify(x => x.GetByIds(idsInput), Times.Once);
        }

        [Fact]
        public async Task GetByIds_DeveRetornarListaVazia_QuandoDataSourceRetornaVazio()
        {

            var idsInput = new List<Guid> { Guid.NewGuid() };

            _dataSourceMock.Setup(x => x.GetByIds(idsInput))
                .ReturnsAsync(new List<IngredientDto>());


            var result = await _gateway.GetByIds(idsInput);


            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetById_DeveRetornarIngrediente_QuandoEncontrado()
        {

            var id = Guid.NewGuid();
            var expectedIngredient = new IngredientDto(id, DateTime.Now, "Alface", 0.50m);

            _dataSourceMock.Setup(x => x.GetById(id))
                .ReturnsAsync(expectedIngredient);


            var result = await _gateway.GetById(id);


            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedIngredient);

            _dataSourceMock.Verify(x => x.GetById(id), Times.Once);
        }

        [Fact]
        public async Task GetById_DeveRetornarNull_QuandoNaoEncontrado()
        {

            var id = Guid.NewGuid();

            _dataSourceMock.Setup(x => x.GetById(id))
                .ReturnsAsync((IngredientDto?)null);


            var result = await _gateway.GetById(id);


            result.Should().BeNull();
        }
    }
}