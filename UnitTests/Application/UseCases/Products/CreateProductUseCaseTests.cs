using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Application.UseCases.Products;
using Application.UseCases.Products.Command;
using Domain.Entities.Aggregates.AggregateProduct;
using FluentAssertions;
using Moq;
using Shared.DTO.Categorie;
using Shared.DTO.Ingredient;
using Shared.DTO.Product.Input;
using Shared.DTO.Product.Request;
using Xunit;

namespace Products.UnitTests.Application.UseCases
{
    public class CreateProductUseCaseTests
    {

        private readonly Mock<IProductDataSource> _productDataSourceMock;
        private readonly Mock<ICategorieDataSource> _categorieDataSourceMock;
        private readonly Mock<IIngredientDataSource> _ingredientDataSourceMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;


        private readonly ProductGateway _productGateway;
        private readonly CategorieGateway _categorieGateway;
        private readonly IngredientGateway _ingredientGateway;


        private readonly CreateProductUseCase _useCase;

        public CreateProductUseCaseTests()
        {
            _productDataSourceMock = new Mock<IProductDataSource>();
            _categorieDataSourceMock = new Mock<ICategorieDataSource>();
            _ingredientDataSourceMock = new Mock<IIngredientDataSource>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();

            _productGateway = ProductGateway.Create(_productDataSourceMock.Object);
            _categorieGateway = CategorieGateway.Create(_categorieDataSourceMock.Object);
            _ingredientGateway = IngredientGateway.Create(_ingredientDataSourceMock.Object);

            _useCase = CreateProductUseCase.Create(
                _productGateway,
                _categorieGateway,
                _ingredientGateway,
                _fileStorageServiceMock.Object
            );
        }

        [Fact]
        public async Task Run_DeveCriarProdutoSimples_QuandoCategoriaExiste()
        {

            var categoriaId = Guid.NewGuid();
            var command = new ProductCommand(
                name: "Coca-Cola",
                price: 5.00m,
                description: "Refrigerante",
                categorieId: categoriaId,
                ingredients: null,
                images: null
            );


            var categoriaDto = new CategorieDto(categoriaId, "Bebida", false, DateTime.Now);
            _categorieDataSourceMock.Setup(x => x.GetCategorieById(categoriaId))
                .ReturnsAsync(categoriaDto);


            _productDataSourceMock.Setup(x => x.Create(It.IsAny<ProductInputDto>()))
                .Returns(Task.CompletedTask);


            var result = await _useCase.Run(command);


            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.Name.Should().Be("Coca-Cola");


            _productDataSourceMock.Verify(x => x.Create(It.IsAny<ProductInputDto>()), Times.Once);
        }

        [Fact]
        public async Task Run_DeveCriarProdutoLanche_QuandoIngredientesExistem()
        {

            var categoriaId = Guid.NewGuid();
            var ingredienteId = Guid.NewGuid();

            var command = new ProductCommand(
                name: "X-Burger",
                price: 20.00m,
                description: "Lanche",
                categorieId: categoriaId,
                ingredients: new List<ProductIngredientRequestDto>
                {
                    new ProductIngredientRequestDto { Id = ingredienteId, Quantity = 1 }
                },
                images: null
            );


            var categoriaDto = new CategorieDto(categoriaId, "Lanche", true, DateTime.Now);
            _categorieDataSourceMock.Setup(x => x.GetCategorieById(categoriaId))
                .ReturnsAsync(categoriaDto);


            var ingredienteDto = new IngredientDto(ingredienteId, DateTime.Now, "Carne", 2.0m);
            _ingredientDataSourceMock.Setup(x => x.GetByIds(It.IsAny<List<Guid>>()))
                .ReturnsAsync(new List<IngredientDto> { ingredienteDto });


            _productDataSourceMock.Setup(x => x.Create(It.IsAny<ProductInputDto>()))
                .Returns(Task.CompletedTask);


            var result = await _useCase.Run(command);


            result.Should().NotBeNull();
            result.IsLanche.Should().BeTrue();
            result.ProductIngredients.Should().HaveCount(1);

            _productDataSourceMock.Verify(x => x.Create(It.IsAny<ProductInputDto>()), Times.Once);
        }

        [Fact]
        public async Task Run_DeveSalvarImagens_QuandoInformadas()
        {

            var categoriaId = Guid.NewGuid();
            var command = new ProductCommand(
                name: "Produto Foto",
                price: 10m,
                description: "Desc",
                categorieId: categoriaId,
                ingredients: null,
                images: new List<ProductImageRequestDto>
                {
                    new ProductImageRequestDto { Name = "foto1", Blob = new byte[] { 1, 2 } }
                }
            );


            _categorieDataSourceMock.Setup(x => x.GetCategorieById(categoriaId))
                .ReturnsAsync(new CategorieDto(categoriaId, "Geral", false, DateTime.Now));


            _fileStorageServiceMock.Setup(x => x.SaveFilesAsync(It.IsAny<List<byte[]>>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()))
                .ReturnsAsync(new List<string> { "path/foto1.png" });


            await _useCase.Run(command);


            _fileStorageServiceMock.Verify(x => x.SaveFilesAsync(
                It.IsAny<List<byte[]>>(),
                It.IsAny<List<string>>(),
                It.IsAny<List<string>>()), Times.Once);
        }
 

        [Fact]
        public async Task Run_DeveLancarErro_QuandoIngredientesNaoExistem()
        {
            var categoriaId = Guid.NewGuid();
            var ingExistente = Guid.NewGuid();
            var ingInexistente = Guid.NewGuid();

            var command = new ProductCommand(
                Guid.NewGuid(),
                name: "X-Bugado",
                price: 10m,
                categorieId: categoriaId,
                description: "Desc",
                null,
                ingredients: new List<ProductIngredientRequestDto>
                {
                    new ProductIngredientRequestDto { Id = ingExistente, Quantity = 1 },
                    new ProductIngredientRequestDto { Id = ingInexistente, Quantity = 1 }
                }
            );

            _categorieDataSourceMock.Setup(x => x.GetCategorieById(categoriaId))
                .ReturnsAsync(new CategorieDto(categoriaId, "Lanche", true, DateTime.Now));

            _ingredientDataSourceMock.Setup(x => x.GetByIds(It.IsAny<List<Guid>>()))
                .ReturnsAsync(new List<IngredientDto>
                {
                    new IngredientDto(ingExistente, DateTime.Now, "Real", 1m)
                });

            Func<Task> act = async () => await _useCase.Run(command);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"Error:Os seguintes ingredientes não foram encontrados: {ingInexistente}");
        }
    }
}