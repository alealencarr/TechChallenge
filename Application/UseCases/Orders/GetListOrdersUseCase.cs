using Application.Gateways;
using Domain.Entities.Aggregates.AggregateOrder;

namespace Application.UseCases.Orders
{
    public class GetListOrdersUseCase
    {
        OrderGateway _gateway = null;
        public static GetListOrdersUseCase Create(OrderGateway gateway)
        {
            return new GetListOrdersUseCase(gateway);
        }

        private GetListOrdersUseCase(OrderGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<List<Order>> Run()
        {
            try
            {
                var orders = await _gateway.GetListOrders();

                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
