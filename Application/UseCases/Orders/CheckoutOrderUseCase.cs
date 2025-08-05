using Application.Gateways;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Entities.Aggregates.AggregateOrder;
using Domain.Entities.Aggregates.AggregateProduct;
using System.Diagnostics.Contracts;
using System.Net;

namespace Application.UseCases.Orders
{
    public class CheckoutOrderUseCase
    {
        OrderGateway _gateway = null;
        PaymentGateway _gatewayPayment = null;
        IFileStorageService _fileStorageService = null;
        public static CheckoutOrderUseCase Create(OrderGateway gateway, PaymentGateway paymentGateway, IFileStorageService fileStorageService)
        {
            return new CheckoutOrderUseCase(gateway, paymentGateway, fileStorageService);
        }

        private CheckoutOrderUseCase(OrderGateway gateway, PaymentGateway paymentGateway, IFileStorageService fileStorageService)
        {
            _gateway = gateway;
            _gatewayPayment = paymentGateway;
            _fileStorageService = fileStorageService;
        }

        public async Task<Payment?> Run(Guid id)
        {
            try
            {
                var order = await _gateway.GetById(id);

                if (order is null)
                    throw new Exception($"Error: Order not find by Id.");

                if (order.StatusOrder == Domain.Entities.Enums.EStatusOrder.Cancelado)
                    throw new Exception($"Este pedido teve o pagamento recusado e foi cancelado. Não é possivel realizar alterações, nem gerar um novo pagamento para ele.");

                if (order.StatusOrder == Domain.Entities.Enums.EStatusOrder.Recebido)
                    throw new Exception($"Este pedido já foi pago e recebido pela cozinha.");

                if (order.Payment is not null)
                    throw new Exception($"O pagamento deste pedido está sendo processado, favor aguardar.");

                var payment = await _gatewayPayment.GenerateQrCodeAsync(order.Id, order.Price);

                if (payment is null)
                    throw new Exception($"Error: Generate payment error.");

                await _gateway.UpdatePayment(payment);
                await _fileStorageService.SaveFileAsync(payment.QrBytes, payment.FileName, payment.PathRoot);

                return payment;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
