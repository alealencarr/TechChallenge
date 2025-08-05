using Application.Interfaces.DataSources;
using Domain.Entities;

namespace Application.Gateways
{
    public class PaymentGateway
    {
        private IPaymentDataSource _dataSource;

        private PaymentGateway(IPaymentDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        public static PaymentGateway Create(IPaymentDataSource dataSource)
        {
            return new PaymentGateway(dataSource);
        }

        public async Task<Payment?> GenerateQrCodeAsync(Guid id, decimal amount)
        {
            var paymentQrCode = await _dataSource.GenerateQrCodeAsync(id, amount);

            var payment = new Payment(id, amount, paymentQrCode);

            return payment;
        }

    }
}
