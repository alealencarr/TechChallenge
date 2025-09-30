using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Application.Presenter.Products;
using Application.UseCases.Products;
using Application.UseCases.Products.Command;
using Shared.DTO.Product.Output;
using Shared.DTO.Product.Request;
using Shared.Result;

namespace Application.Controllers.Products
{
    public class ProductController
    {
        private IProductDataSource _dataSource;
        private ICategorieDataSource? _dataSourceCategorie;
        private IIngredientDataSource? _dataSourceIngredient;
        private IFileStorageService _fileStorage;
        public ProductController(IProductDataSource dataSource, IIngredientDataSource? ingredientDataSource, ICategorieDataSource? categorieDataSource, IFileStorageService fileStorage)
        {
            _dataSource = dataSource;
            _dataSourceCategorie = categorieDataSource;
            _dataSourceIngredient = ingredientDataSource;
            _fileStorage = fileStorage;
        }

        public ProductController(IProductDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        public async Task<ICommandResult<ProductOutputDto>> CreateProduct(ProductRequestDto productRequestDto)
        {
            ProductPresenter productPresenter = new("Produto cadastrado!");

            try
            {
                var command = new ProductCommand(productRequestDto.Name, productRequestDto.Price, productRequestDto.CategorieId, productRequestDto.Description, productRequestDto.Images, productRequestDto.Ingredients); 

                var productGateway = ProductGateway.Create(_dataSource);
                var ingredientGateway = IngredientGateway.Create(_dataSourceIngredient!);
                var categorieGateway = CategorieGateway.Create(_dataSourceCategorie!);

                var useCaseCreate = CreateProductUseCase.Create(productGateway, categorieGateway, ingredientGateway, _fileStorage);
                var productEntity = await useCaseCreate.Run(command);

                var dtoRetorno = productPresenter.TransformObject(productEntity);

                return dtoRetorno;
            }
            catch (Exception ex)
            {
                return productPresenter.Error<ProductOutputDto>(ex.Message);
            }
        }

        public async Task<ICommandResult<ProductOutputDto>> UpdateProduct(ProductRequestDto productRequestDto, Guid id)
        {
            ProductPresenter productPresenter = new("Produto alterado!");

            try
            {
                var command = new ProductCommand(id, productRequestDto.Name, productRequestDto.Price, productRequestDto.CategorieId, productRequestDto.Description, productRequestDto.Images, productRequestDto.Ingredients);


                var productGateway = ProductGateway.Create(_dataSource);
                var ingredientGateway = IngredientGateway.Create(_dataSourceIngredient!);
                var categorieGateway = CategorieGateway.Create(_dataSourceCategorie!);

                var useCaseCreate = UpdateProductUseCase.Create(productGateway, categorieGateway, ingredientGateway, _fileStorage);

                var productEntity = await useCaseCreate.Run(command);

                var dtoRetorno = productPresenter.TransformObject(productEntity);

                return dtoRetorno;
            }
            catch (Exception ex)
            {
                return productPresenter.Error<ProductOutputDto>(ex.Message);
            }
        }

        public async Task<ICommandResult> DeleteProduct(Guid id)
        {
            ProductPresenter productPresenter = new("Produto excluido!");

            try
            {
                var productGateway = ProductGateway.Create(_dataSource);

                var useCaseCreate = DeleteProductUseCase.Create(productGateway, _fileStorage);

                await useCaseCreate.Run(id);

                return productPresenter.RetornoSucess();
            }
            catch (Exception ex)
            {
                return productPresenter.Error(ex.Message);
            }
        }

        public async Task<ICommandResult<ProductOutputDto?>> GetProductById(Guid id)
        {
            ProductPresenter productPresenter = new("Produto encontrado!");

            try
            {
                var productGateway = ProductGateway.Create(_dataSource);
                var useCase = GetProductByIdUseCase.Create(productGateway);
                var product = await useCase.Run(id);

                return product is null ? productPresenter.Error<ProductOutputDto?>("Product not found.") : productPresenter.TransformObject(product);
            }
            catch (Exception ex)
            {
                return productPresenter.Error<ProductOutputDto?>(ex.Message);
            }
        }

        public async Task<ICommandResult<List<ProductOutputDto>>> GetProductsByCategorie(string? id, string? name)
        {
            ProductPresenter productPresenter = new("Products encontrados!");

            try
            {
                var productGateway = ProductGateway.Create(_dataSource);
                var useCase = GetProductByCategorieUseCase.Create(productGateway);
                var products = await useCase.Run(id, name);

                return products is null ? productPresenter.Error<List<ProductOutputDto>>("Products not found.") : productPresenter.TransformList(products);
            }
            catch (Exception ex)
            {
                return productPresenter.Error<List<ProductOutputDto>>(ex.Message);
            }
        }

    }
}
