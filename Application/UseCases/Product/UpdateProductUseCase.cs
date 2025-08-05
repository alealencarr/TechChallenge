using Application.Gateways;
using Application.Interfaces.Services;
using Application.UseCases.Products.Command;
using Domain.Entities;
using Domain.Entities.Aggregates.AggregateProduct;

namespace Application.UseCases.Products
{

    public class UpdateProductUseCase
    {
        ProductGateway _gateway = null;
        CategorieGateway _gatewayCategorie = null;
        IngredientGateway _gatewayIngredient = null;
        private Categorie _categorie = null;
        ProductCommand _command = null;
        IFileStorageService _fileStorageService;
        public static UpdateProductUseCase Create(ProductGateway gateway, CategorieGateway categorieGateway, IngredientGateway ingredientGateway, IFileStorageService fileStorageService)
        {
            return new UpdateProductUseCase(gateway, categorieGateway, ingredientGateway, fileStorageService);
        }
        private UpdateProductUseCase(ProductGateway gateway, CategorieGateway categorieGateway, IngredientGateway ingredientGateway, IFileStorageService fileStorageService)
        {
            _gateway = gateway;
            _gatewayCategorie = categorieGateway;
            _gatewayIngredient = ingredientGateway;
            _fileStorageService = fileStorageService;
        }

        public async Task<Product> Run(ProductCommand product)
        {
            try
            {
                _command = product;
                var productExists = await _gateway.GetById(product.Id);

                await CommandIsValidForEntity();

                if (productExists is null)
                    throw new Exception($"Error: Product not find by Id.");

                var productsIngredients = (product.Ingredients is null ? new List<ProductIngredient>() :
                product.Ingredients?.Select(x => new ProductIngredient(x.Id, x.Quantity)).ToList());
                var productImages = (product.Images is null ? new List<ProductImage>() :
                    product.Images?.Select(x => new ProductImage(x.Blob, x.Name)).ToList());

                var productUpdate = Product.Update(product.Id, product.Name, product.Price, product.CategorieId, product.Description, productsIngredients!, productImages!, _categorie);

                await _gateway.UpdateProduct(productUpdate);

                _fileStorageService.CleanFolder($"produtos/imagens/produto-{productUpdate.Id}");

                if (productUpdate.ProductImages!.Any())
                {
                    await _fileStorageService.SaveFilesAsync(productUpdate.ProductImages.Select(x => x.Blob).ToList(), productUpdate.ProductImages.Select(x => x.FileName).ToList(), productUpdate.ProductImages.Select(x => x.ImagePath).ToList());
                }

                return (await _gateway.GetById(product.Id));
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }

        private async Task CommandIsValidForEntity()
        {
            if (_command.Ingredients is null || _command.Ingredients.Count == 0)
            {
                await ValidationAnyCategories();
            }
            else
            {
                await ValidationCategoriesAndIngredients();
            }
        }

        private async Task ValidationCategoriesAndIngredients()
        {
            var validationErrors = new List<string>();

            List<Guid> ingredientesIds = _command.Ingredients!
            .Select(i => i.Id)
            .Distinct()
            .ToList();

            var _categoria = await _gatewayCategorie.GetById(_command.CategorieId);
            var ingredientesDb = await _gatewayIngredient.GetByIds(ingredientesIds);

            //await Task.WhenAll(taskCategorie, taskIngredients);

            //var _categoria = await taskCategorie;
            //var ingredientesDb = await taskIngredients;

            if (_categoria is null)
            {
                validationErrors.Add($"Categoria com ID {_command.CategorieId} não encontrada.");
            }

            if (ingredientesIds.Count != ingredientesDb.Count)
            {
                var ingredientesEncontradosIds = ingredientesDb.Select(i => i.Id).ToHashSet();
                var ingredientesNaoEncontrados = ingredientesIds
                    .Where(id => !ingredientesEncontradosIds.Contains(id))
                    .ToList();

                if (ingredientesNaoEncontrados.Any())
                {
                    var idsFaltando = string.Join(", ", ingredientesNaoEncontrados);
                    validationErrors.Add($"Os seguintes ingredientes não foram encontrados: {idsFaltando}");
                }
            }

            if (validationErrors.Any())
            {
                var mensagemDeErroConsolidada = string.Join(" | ", validationErrors);
                throw new Exception(mensagemDeErroConsolidada);
            }

            _categorie = _categoria!;
        }

        private async Task ValidationAnyCategories()
        {
            var validationErrors = new List<string>();

            var _categoria = await _gatewayCategorie.GetById(_command.CategorieId);

            if (_categoria is null)
            {
                throw new Exception($"Categoria com ID {_command.CategorieId} não encontrada.");
            }

            _categorie = _categoria!;
        }

    }
}
