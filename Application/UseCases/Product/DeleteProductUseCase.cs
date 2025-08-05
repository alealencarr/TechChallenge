using Application.Gateways;

namespace Application.UseCases.Products
{

    public class DeleteProductUseCase
    {
        ProductGateway _gateway = null;
        public static DeleteProductUseCase Create(ProductGateway gateway)
        {
            return new DeleteProductUseCase(gateway);
        }

        private DeleteProductUseCase(ProductGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task Run(Guid id)
        {
            try
            {
                await _gateway.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
