using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Application.UseCases.Products;
using FluentAssertions;
using Moq;
using Shared.DTO.Product.Input;

namespace Products.UnitTests.Application.UseCases
{
    public class DeleteProductUseCaseTests
    {
        private readonly Mock<IProductDataSource> _dataSourceMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly ProductGateway _gateway;
        private readonly DeleteProductUseCase _useCase;

        public DeleteProductUseCaseTests()
        {
            _dataSourceMock = new Mock<IProductDataSource>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();

            _gateway = ProductGateway.Create(_dataSourceMock.Object);

            _useCase = DeleteProductUseCase.Create(_gateway, _fileStorageServiceMock.Object);
        }

        [Fact]
        public async Task Run_DeveDeletarProduto_ELimparPasta_QuandoProdutoExiste()
        {
            var id = Guid.NewGuid();

            var productDto = new ProductInputDto(id, DateTime.Now, "Teste", "Desc", 10m, Guid.NewGuid(), newList<ProductImageInputDto>(), newList<ProductIngredientInputDto>(), false);
            _dataSourceMock.Setup(x => x.GetById(id))
                           .ReturnsAsync(productDto);

            _dataSourceMock.Setup(x => x.Delete(id))
                           .Returns(Task.CompletedTask);

            _fileStorageServiceMock.Setup(x => x.CleanFolder(It.IsAny<string>()));

            await _useCase.Run(id);

            _dataSourceMock.Verify(x => x.Delete(id), Times.Once);

            _fileStorageServiceMock.Verify(x => x.CleanFolder($"produtos/imagens/produto-{id}"), Times.Once);
        }

        [Fact]
        public async Task Run_DeveLancarErro_QuandoProdutoNaoEncontrado()
        {
            var id = Guid.NewGuid();

            _dataSourceMock.Setup(x => x.GetById(id))
                           .ReturnsAsync((ProductInputDto?)null);

            Func<Task> act = async () => await _useCase.Run(id);

            await act.Should().ThrowAsync<Exception>()
                     .WithMessage("Error:Error: Product not find by Id.");

            _dataSourceMock.Verify(x => x.Delete(It.IsAny<Guid>()), Times.Never);
            _fileStorageServiceMock.Verify(x => x.CleanFolder(It.IsAny<string>()), Times.Never);
        }


        private List<T> newList<T>() => new List<T>();
    }
}