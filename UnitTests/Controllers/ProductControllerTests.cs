using Application.Controllers.Products;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using FluentAssertions;
using Moq;
using Shared.DTO.Categorie;
using Shared.DTO.Product.Input;
using Shared.DTO.Product.Request;

namespace Products.UnitTests.Application.Controllers
{
    public class ProductControllerTests
    {

        private readonly Mock<IProductDataSource> _productDataSourceMock;
        private readonly Mock<ICategorieDataSource> _categorieDataSourceMock;
        private readonly Mock<IIngredientDataSource> _ingredientDataSourceMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;

        public ProductControllerTests()
        {
            _productDataSourceMock = new Mock<IProductDataSource>();
            _categorieDataSourceMock = new Mock<ICategorieDataSource>();
            _ingredientDataSourceMock = new Mock<IIngredientDataSource>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();
        }



        [Fact]
        public async Task CreateProduct_ShouldCreateProduct_WhenValid()
        {

            var controller = new ProductController(
                _productDataSourceMock.Object,
                _ingredientDataSourceMock.Object,
                _categorieDataSourceMock.Object,
                _fileStorageServiceMock.Object
            );

            var catId = Guid.NewGuid();
            var request = new ProductRequestDto
            {
                Name = "Coke",
                Price = 5.0m,
                CategorieId = catId,
                Description = "Soda",
                Ingredients = null,
                Images = null
            };


            _categorieDataSourceMock.Setup(x => x.GetCategorieById(catId))
                .ReturnsAsync(new CategorieDto(catId, "Drinks", false, DateTime.Now));

            _productDataSourceMock.Setup(x => x.Create(It.IsAny<ProductInputDto>()))
                .Returns(Task.CompletedTask);


            var result = await controller.CreateProduct(request);


            result.Succeeded.Should().BeTrue();
            result.Messages[0].Should().Be("Produto cadastrado!");
            result.Data.Should().NotBeNull();
            result.Data.Name.Should().Be("Coke");
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnError_WhenCategoryNotFound()
        {
            var controller = new ProductController(
                _productDataSourceMock.Object, _ingredientDataSourceMock.Object, _categorieDataSourceMock.Object, _fileStorageServiceMock.Object
            );

            var catId = Guid.NewGuid();
            var request = new ProductRequestDto { CategorieId = catId, Name = "Test", Price = 10, Description = "Desc" };

            _categorieDataSourceMock.Setup(x => x.GetCategorieById(catId))
                .ReturnsAsync((CategorieDto?)null);

            var result = await controller.CreateProduct(request);

            result.Succeeded.Should().BeFalse();
            result.Messages[0].Should().Contain($"Categoria com ID {catId} não encontrada");
        }



        [Fact]
        public async Task UpdateProduct_ShouldUpdateProduct_WhenValid()
        {

            var controller = new ProductController(
                _productDataSourceMock.Object, _ingredientDataSourceMock.Object, _categorieDataSourceMock.Object, _fileStorageServiceMock.Object
            );

            var id = Guid.NewGuid();
            var catId = Guid.NewGuid();
            var request = new ProductRequestDto
            {
                Name = "Updated Coke",
                Price = 6.0m,
                CategorieId = catId,
                Description = "New Desc"
            };

            var existingProduct = new ProductInputDto(id, DateTime.Now, "Old Name", "Old Desc", 5m, catId, new List<ProductImageInputDto>(),
                new List<ProductIngredientInputDto>(), false);
            _productDataSourceMock.Setup(x => x.GetById(id)).ReturnsAsync(existingProduct);


            _categorieDataSourceMock.Setup(x => x.GetCategorieById(catId))
                .ReturnsAsync(new CategorieDto(catId, "Drinks", false, DateTime.Now));


            _productDataSourceMock.Setup(x => x.Update(It.IsAny<ProductInputDto>()))
                .Returns(Task.CompletedTask);


            var result = await controller.UpdateProduct(request, id);


            result.Succeeded.Should().BeTrue();
            result.Messages[0].Should().Be("Produto alterado!");
            result.Data.Name.Should().Be("Old Name");
        }



        [Fact]
        public async Task DeleteProduct_ShouldDeleteAndCleanStorage_WhenExists()
        {


            var controller = new ProductController(
                _productDataSourceMock.Object,
                _ingredientDataSourceMock.Object,
                _categorieDataSourceMock.Object,
                _fileStorageServiceMock.Object
            );

            var id = Guid.NewGuid();
            var productDto = new ProductInputDto(id, DateTime.Now, "Test", "Desc", 10m, Guid.NewGuid(), new List<ProductImageInputDto>(),
                new List<ProductIngredientInputDto>(), false);

            _productDataSourceMock.Setup(x => x.GetById(id)).ReturnsAsync(productDto);
            _productDataSourceMock.Setup(x => x.Delete(id)).Returns(Task.CompletedTask);


            var result = await controller.DeleteProduct(id);


            result.Succeeded.Should().BeTrue();
            result.Messages[0].Should().Be("Produto excluido!");


            _fileStorageServiceMock.Verify(x => x.CleanFolder(It.Is<string>(s => s.Contains(id.ToString()))), Times.Once);
        }



        [Fact]
        public async Task GetProductById_ShouldReturnProduct_WhenFound()
        {

            var controller = new ProductController(_productDataSourceMock.Object);
            var id = Guid.NewGuid();
            var dto = new ProductInputDto(id, DateTime.Now, "Coke", "Desc", 5m, Guid.NewGuid(), new List<ProductImageInputDto>(),
                new List<ProductIngredientInputDto>(), false);

            _productDataSourceMock.Setup(x => x.GetById(id)).ReturnsAsync(dto);


            var result = await controller.GetProductById(id);


            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnError_WhenNotFound()
        {

            var controller = new ProductController(_productDataSourceMock.Object);
            var id = Guid.NewGuid();
            _productDataSourceMock.Setup(x => x.GetById(id)).ReturnsAsync((ProductInputDto?)null);


            var result = await controller.GetProductById(id);


            result.Succeeded.Should().BeFalse();
            result.Messages[0].Should().Be("Product not found.");
        }



        [Fact]
        public async Task GetProductsByCategorie_ShouldReturnList_WhenFound()
        {

            var controller = new ProductController(_productDataSourceMock.Object);
            var catId = Guid.NewGuid().ToString();

            var list = new List<ProductInputDto>
            {
                new ProductInputDto(Guid.NewGuid(), DateTime.Now, "P1", "D", 10m, Guid.Parse(catId), new List<ProductImageInputDto>(),
                new List<ProductIngredientInputDto>(), false)
            };

            _productDataSourceMock.Setup(x => x.GetByCategorie(catId, It.IsAny<string?>()))
                .ReturnsAsync(list);


            var result = await controller.GetProductsByCategorie(catId, null);


            result.Succeeded.Should().BeTrue();
            result.Data.Should().HaveCount(1);
        }
    }
}