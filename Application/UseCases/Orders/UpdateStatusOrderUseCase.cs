using Application.Gateways;
using Domain.Entities.Aggregates.AggregateOrder;

namespace Application.UseCases.Orders
{
    public class UpdateStatusOrderUseCase
    {
        OrderGateway _gateway = null;
        public static UpdateStatusOrderUseCase Create(OrderGateway gateway)
        {
            return new UpdateStatusOrderUseCase(gateway);
        }

        private UpdateStatusOrderUseCase(OrderGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Order?> Run(Guid id)
        {
            try
            {
                var order = await _gateway.GetById(id);

                if (order is null)
                    throw new Exception($"Error: Order not find by Id.");

                if (order.StatusOrder == Domain.Entities.Enums.EStatusOrder.EmAberto)
                    throw new Exception($"É necessário primeiro fazer o checkout do pedido.");

                if (order.StatusOrder == Domain.Entities.Enums.EStatusOrder.Cancelado)
                    throw new Exception($"Este pedido teve o pagamento recusado e foi cancelado. Não é possivel realizar alterações, nem gerar um novo pagamento para ele.");

                if (order.StatusOrder == Domain.Entities.Enums.EStatusOrder.Finalizado)
                    throw new Exception($"Este pedido já foi finalizado.");

                order.UpdateStatus();

                await _gateway.UpdateStatusOrder(order);

                return order;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
