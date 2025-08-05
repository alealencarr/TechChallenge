using Application.Gateways;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Entities.Enums;

namespace Application.UseCases.Orders
{
    public class UpdatePaymentOrderUseCase
    {
        OrderGateway _gateway = null;
 
        public static UpdatePaymentOrderUseCase Create(OrderGateway gateway)
        {
            return new UpdatePaymentOrderUseCase(gateway);
        }

        private UpdatePaymentOrderUseCase(OrderGateway gateway )
        {
            _gateway = gateway; 
        }

        public async Task<Payment?> Run(Guid orderId, Guid id, EPaymentStatus status, decimal amount)
        {            
            try
            {
                var order = await _gateway.GetById(orderId);

                if (order is null)
                    throw new Exception($"Error: Order not find by Id.");

                if (order.StatusOrder == Domain.Entities.Enums.EStatusOrder.Recebido)
                    throw new Exception($"Este pedido já foi pago e recebido pela cozinha.");

                var payment = order.Payment;
                payment.PaymentStatus = status;                
 
                await _gateway.UpdatePaymentAndStatusOrder(payment, (status == EPaymentStatus.Paid ? EStatusOrder.Recebido :  EStatusOrder.Cancelado) );

                return payment;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
