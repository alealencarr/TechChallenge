using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Application.UseCases.Products;
using Application.UseCases.Products.Command;
using FluentAssertions;
using Moq;
using Shared.DTO.Categorie;
using Shared.DTO.Ingredient;
using Shared.DTO.Product.Input;
using Shared.DTO.Product.Request;

namespace Products.UnitTests.Application.UseCases
{
    public class UpdateProductUseCaseTests
    {
        private readonly Mock<IProductDataSource> _productDataSourceMock;
        private readonly Mock<ICategorieDataSource> _categorieDataSourceMock;
        private readonly Mock<IIngredientDataSource> _ingredientDataSourceMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;

        private readonly ProductGateway _productGateway;
        private readonly CategorieGateway _categorieGateway;
        private readonly IngredientGateway _ingredientGateway;

        private readonly UpdateProductUseCase _useCase;

        public UpdateProductUseCaseTests()
        {
            _productDataSourceMock = new Mock<IProductDataSource>();
            _categorieDataSourceMock = new Mock<ICategorieDataSource>();
            _ingredientDataSourceMock = new Mock<IIngredientDataSource>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();

            _productGateway = ProductGateway.Create(_productDataSourceMock.Object);
            _categorieGateway = CategorieGateway.Create(_categorieDataSourceMock.Object);
            _ingredientGateway = IngredientGateway.Create(_ingredientDataSourceMock.Object);

            _useCase = UpdateProductUseCase.Create(
                _productGateway,
                _categorieGateway,
                _ingredientGateway,
                _fileStorageServiceMock.Object
            );
        }

        [Fact]
        public async Task Run_DeveAtualizarProduto_QuandoDadosValidos()
        {
            var productId = Guid.NewGuid();
            var categoriaId = Guid.NewGuid();

            var command = new ProductCommand(
                id: productId,
                name: "Coca-Cola Zero",
                price: 6.00m,
                description: "Refrigerante sem açúcar",
                categorieId: categoriaId,
                ingredients: null,
                images: null
            );

            var existingProduct = new ProductInputDto(productId, DateTime.UtcNow, "Coca-Cola", "Desc", 5.00m, categoriaId,
                new List<ProductImageInputDto>(), new List<ProductIngredientInputDto>(), false);

            _productDataSourceMock.Setup(x => x.GetById(productId))
                .ReturnsAsync(existingProduct);

            var categoriaDto = new CategorieDto(categoriaId, "Bebida", false, DateTime.Now);
            _categorieDataSourceMock.Setup(x => x.GetCategorieById(categoriaId))
                .ReturnsAsync(categoriaDto);

            _productDataSourceMock.Setup(x => x.Update(It.IsAny<ProductInputDto>()))
                .Returns(Task.CompletedTask);

            var result = await _useCase.Run(command);

            result.Should().NotBeNull();
            result.Id.Should().Be(productId);
            _productDataSourceMock.Verify(x => x.Update(It.IsAny<ProductInputDto>()), Times.Once);
            _fileStorageServiceMock.Verify(x => x.CleanFolder(It.Is<string>(s => s.Contains(productId.ToString()))), Times.Once);
        }

        [Fact]
        public async Task Run_DeveLancarErro_QuandoProdutoNaoExiste()
        {

            var productId = Guid.NewGuid();
            var categorieId = Guid.NewGuid();
            var command = new ProductCommand(productId, "Nome", 10m, categorieId, "Desc", null, null);


            _productDataSourceMock.Setup(x => x.GetById(productId))
                .ReturnsAsync((ProductInputDto)null);

            Func<Task> act = async () => await _useCase.Run(command);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"Error:Categoria com ID {categorieId} não encontrada.");

            _productDataSourceMock.Verify(x => x.Update(It.IsAny<ProductInputDto>()), Times.Never);
        }

        [Fact]
        public async Task Run_DeveAtualizarImagens_QuandoNovasImagensInformadas()
        {
            var productId = Guid.NewGuid();
            var categoriaId = Guid.NewGuid();

            var command = new ProductCommand(
                id: productId,
                name: "Produto Foto",
                price: 10m,
                description: "Desc",
                categorieId: categoriaId,
                ingredients: null,
                images: new List<ProductImageRequestDto>
                {
                    new ProductImageRequestDto { Name = "nova-foto", Blob = new byte[] { 1, 2, 3 } }
                }
            );

            var existingProduct = new ProductInputDto(productId, DateTime.UtcNow, "Coca-Cola", "Desc", 5.00m, categoriaId,
            new List<ProductImageInputDto>(), new List<ProductIngredientInputDto>(), false);

            _productDataSourceMock.Setup(x => x.GetById(productId)).ReturnsAsync(existingProduct);

            _categorieDataSourceMock.Setup(x => x.GetCategorieById(categoriaId))
               .ReturnsAsync(new CategorieDto(categoriaId, "Geral", false, DateTime.Now));

            _fileStorageServiceMock.Setup(x => x.SaveFilesAsync(It.IsAny<List<byte[]>>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()))
               .ReturnsAsync(new List<string> { "path/nova-foto.png" });

            await _useCase.Run(command);

            _fileStorageServiceMock.Verify(x => x.CleanFolder(It.IsAny<string>()), Times.Once);


            _fileStorageServiceMock.Verify(x => x.SaveFilesAsync(
                It.IsAny<List<byte[]>>(),
                It.IsAny<List<string>>(),
                It.IsAny<List<string>>()), Times.Once);
        }

        [Fact]
        public async Task Run_DeveLancarErro_QuandoIngredientesNaoEncontradosNoUpdate()
        {

            var productId = Guid.NewGuid();
            var categoriaId = Guid.NewGuid();
            var ingInexistente = Guid.NewGuid();

            var command = new ProductCommand(
                id: productId,
                name: "X-Bugado",
                price: 15m,
                description: "Desc",
                categorieId: categoriaId,
                ingredients: new List<ProductIngredientRequestDto>
                {
                    new ProductIngredientRequestDto { Id = ingInexistente, Quantity = 1 }
                },
                images: null
            );


            var existingProduct = new ProductInputDto(productId, DateTime.UtcNow, "Coca-Cola", "Desc", 5.00m, categoriaId,
              new List<ProductImageInputDto>(), new List<ProductIngredientInputDto>(), false);


            _categorieDataSourceMock.Setup(x => x.GetCategorieById(categoriaId))
                .ReturnsAsync(new CategorieDto(categoriaId, "Lanche", true, DateTime.Now));


            _ingredientDataSourceMock.Setup(x => x.GetByIds(It.IsAny<List<Guid>>()))
                .ReturnsAsync(new List<IngredientDto>());


            Func<Task> act = async () => await _useCase.Run(command);


            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"Error:Os seguintes ingredientes não foram encontrados: {ingInexistente}");

            _productDataSourceMock.Verify(x => x.Update(It.IsAny<ProductInputDto>()), Times.Never);
        }

        [Fact]
        public async Task Run_DeveLancarErro_QuandoCategoriaNaoEncontrada()
        {

            var productId = Guid.NewGuid();
            var categoriaIdInexistente = Guid.NewGuid();

            var command = new ProductCommand(
                id: productId,
                name: "Produto Sem Categoria",
                price: 10m,
                description: "Desc",
                categorieId: categoriaIdInexistente,
                ingredients: null,
                images: null
            );


            var existingProduct = new ProductInputDto(productId, DateTime.UtcNow, "Coca-Cola", "Desc", 5.00m, Guid.Empty,
      new List<ProductImageInputDto>(), new List<ProductIngredientInputDto>(), false);



            _categorieDataSourceMock.Setup(x => x.GetCategorieById(categoriaIdInexistente))
                .ReturnsAsync((CategorieDto)null);


            Func<Task> act = async () => await _useCase.Run(command);


            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"Error:Categoria com ID {categoriaIdInexistente} não encontrada.");
        }
    }
}