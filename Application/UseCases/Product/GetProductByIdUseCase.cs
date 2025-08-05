using Application.Gateways;
using Domain.Entities;
using Domain.Entities.Aggregates.AggregateProduct;

namespace Application.UseCases.Products
{
    public class GetProductByIdUseCase
    {
        ProductGateway _gateway = null;
        public static GetProductByIdUseCase Create(ProductGateway gateway)
        {
            return new GetProductByIdUseCase(gateway);
        }

        private GetProductByIdUseCase(ProductGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Product?> Run(Guid id)
        {
            try
            {
                var product = await _gateway.GetById(id);

                return product;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
