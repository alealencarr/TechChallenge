using Application.Gateways;
using Domain.Entities;

namespace Application.UseCases.Orders;

public class GetPaymentStatusOrderUseCase
{
    OrderGateway _gateway = null;
    public static GetPaymentStatusOrderUseCase Create(OrderGateway gateway)
    {
        return new GetPaymentStatusOrderUseCase(gateway);
    }

    private GetPaymentStatusOrderUseCase(OrderGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task<Payment?> Run(Guid id)
    {
        try
        {
            var order = await _gateway.GetById(id);

            if (order is null)
                throw new Exception($"Error: Order not find by Id.");

            if (order.Payment is null)
                throw new Exception($"Este pedido ainda não tem informação de pagamento, favor realize o checkout para consultar as informações de pagamento.");

            return order.Payment;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error:{ex.Message}");
        }
    }

}