using Application.Gateways;
using Domain.Entities.Aggregates.AggregateOrder;

namespace Application.UseCases.Orders
{
    public class GetOrderByIdUseCase
    {
        OrderGateway _gateway = null;
        public static GetOrderByIdUseCase Create(OrderGateway gateway)
        {
            return new GetOrderByIdUseCase(gateway);
        }

        private GetOrderByIdUseCase(OrderGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Order?> Run(Guid id)
        {
            try
            {
                var order = await _gateway.GetById(id);

                return order;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
