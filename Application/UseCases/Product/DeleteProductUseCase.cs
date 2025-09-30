using Application.Gateways;
using Application.Interfaces.Services;
using Domain.Entities.Aggregates.AggregateProduct;

namespace Application.UseCases.Products
{

    public class DeleteProductUseCase
    {
        ProductGateway _gateway = null;
        IFileStorageService _fileStorageService;

        public static DeleteProductUseCase Create(ProductGateway gateway, IFileStorageService fileStorageService)
        {
            return new DeleteProductUseCase(gateway, fileStorageService);
        }

        private DeleteProductUseCase(ProductGateway gateway, IFileStorageService fileStorageService)
        {
            _gateway = gateway;
            _fileStorageService = fileStorageService;
        }

        public async Task Run(Guid id)
        {
            try
            {
                var productExists = await _gateway.GetById(id);

                if (productExists is null)
                    throw new Exception($"Error: Product not find by Id.");

                await _gateway.Delete(id);

                _fileStorageService.CleanFolder($"produtos/imagens/produto-{productExists.Id}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
