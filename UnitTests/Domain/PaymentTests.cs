using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;
using Xunit;

namespace Orders.UnitTests.Domain.Entities
{
    public class PaymentTests
    {
        [Fact]
        public void Deve_Criar_Pagamento_Com_Sucesso()
        {
            var orderId = Guid.NewGuid();
            var valor = 50.00m;
            var qrBytes = new byte[] { 1, 2, 3 };

            var payment = new Payment(orderId, valor, qrBytes);

            payment.Should().NotBeNull();
            payment.Id.Should().NotBeEmpty();
            payment.Amount.Should().Be(valor);
            payment.PaymentStatus.Should().Be(EPaymentStatus.Pending);
            payment.FileName.Should().Contain(orderId.ToString());
        }

        [Fact]
        public void Deve_Lancar_Erro_Se_Valor_Zero()
        {
            Action act = () => new Payment(Guid.NewGuid(), 0m, new byte[] { 1 });

            act.Should().Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'É necessário que o valor seja maior que zero para criar um pagamento.')");
        }

        [Fact]
        public void Deve_Lancar_Erro_Se_QrCode_Vazio()
        {
            Action act = () => new Payment(Guid.NewGuid(), 10m, new byte[0]);

            act.Should().Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'É necessário um QR Code para criar um pagamento.')");
        }
    }
}