using Application.Gateways;
using Domain.Entities.Aggregates.AggregateProduct;

namespace Application.UseCases.Products
{
    public class GetProductByCategorieUseCase
    {
        ProductGateway _gateway = null;
        public static GetProductByCategorieUseCase Create(ProductGateway gateway)
        {
            return new GetProductByCategorieUseCase(gateway);
        }

        private GetProductByCategorieUseCase(ProductGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<List<Product>> Run(string? id, string? name)
        {
            try
            {
                var products = await _gateway.GetByCategorie(id, name);

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
