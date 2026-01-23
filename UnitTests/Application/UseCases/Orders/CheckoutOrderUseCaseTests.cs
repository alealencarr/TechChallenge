using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Application.UseCases.Orders;
using Domain.Entities.Enums;
using FluentAssertions;
using Moq;
using Shared.DTO.Order.Input;
using Shared.DTO.Payment;
using Xunit;

namespace Orders.UnitTests.Application.UseCases
{
    public class CheckoutOrderUseCaseTests
    {
        private readonly Mock<IOrderDataSource> _orderDataSourceMock;
        private readonly Mock<IPaymentDataSource> _paymentDataSourceMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;

        private readonly OrderGateway _orderGateway;
        private readonly PaymentGateway _paymentGateway;

        private readonly CheckoutOrderUseCase _useCase;

        public CheckoutOrderUseCaseTests()
        {
            _orderDataSourceMock = new Mock<IOrderDataSource>();
            _paymentDataSourceMock = new Mock<IPaymentDataSource>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();

            _orderGateway = OrderGateway.Create(_orderDataSourceMock.Object);
            _paymentGateway = PaymentGateway.Create(_paymentDataSourceMock.Object);

            _useCase = CheckoutOrderUseCase.Create(
                _orderGateway,
                _paymentGateway,
                _fileStorageServiceMock.Object
            );
        }

        [Fact]
        public async Task Run_DeveGerarPagamento_ComSucesso()
        {
            var orderId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var valorPedido = 50.00m;
            var qrCodeBytes = new byte[] { 0xAA, 0xBB, 0xCC };
            var caminhoArquivo = "https://storage.com/qrcode.png";

            var orderDto = new OrderInputDto(
                orderId,
                DateTime.Now,
                (int)EStatusOrder.EmAberto, 
                valorPedido,
                customerId,
                new List<ItemOrderInputDto>(), 
                null 
            );

            _orderDataSourceMock.Setup(x => x.GetById(orderId))
                .ReturnsAsync(orderDto);

            _paymentDataSourceMock.Setup(x => x.GenerateQrCodeAsync(orderId, valorPedido))
                .ReturnsAsync(qrCodeBytes);

            _fileStorageServiceMock.Setup(x => x.SaveFileAsync(qrCodeBytes, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(caminhoArquivo);

            _orderDataSourceMock.Setup(x => x.UpdatePayment(It.IsAny<PaymentInputDto>()))
                .Returns(Task.CompletedTask);

            var result = await _useCase.Run(orderId);

            result.Should().NotBeNull();
            result!.QrBytes.Should().BeEquivalentTo(qrCodeBytes);

            _fileStorageServiceMock.Verify(x => x.SaveFileAsync(qrCodeBytes, It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _orderDataSourceMock.Verify(x => x.UpdatePayment(It.IsAny<PaymentInputDto>()), Times.Once);
        }

        [Fact]
        public async Task Run_DeveLancarErro_QuandoPedidoNaoExiste()
        {
            var orderId = Guid.NewGuid();

            _orderDataSourceMock.Setup(x => x.GetById(orderId))
                .ReturnsAsync((OrderInputDto?)null);

            Func<Task> act = async () => await _useCase.Run(orderId);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error:Error: Order not find by Id.");
        }

        [Fact]
        public async Task Run_DeveLancarErro_QuandoPedidoCancelado()
        {
            var orderId = Guid.NewGuid();

            var orderDto = new OrderInputDto(
                orderId, DateTime.Now, (int)EStatusOrder.Cancelado, 10m, null, new List<ItemOrderInputDto>(), null
            );

            _orderDataSourceMock.Setup(x => x.GetById(orderId)).ReturnsAsync(orderDto);

            Func<Task> act = async () => await _useCase.Run(orderId);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error:Este pedido teve o pagamento recusado e foi cancelado. Não é possivel realizar alterações, nem gerar um novo pagamento para ele.");
        }

        [Fact]
        public async Task Run_DeveLancarErro_QuandoPedidoJaRecebido()
        {
            var orderId = Guid.NewGuid();

            var orderDto = new OrderInputDto(
                orderId, DateTime.Now, (int)EStatusOrder.Recebido, 10m, null, new List<ItemOrderInputDto>(), null
            );

            _orderDataSourceMock.Setup(x => x.GetById(orderId)).ReturnsAsync(orderDto);

            Func<Task> act = async () => await _useCase.Run(orderId);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error:Este pedido já foi pago e recebido pela cozinha.");
        }

        [Fact]
        public async Task Run_DeveLancarErro_QuandoPagamentoJaEstaProcessando()
        {
            var orderId = Guid.NewGuid();

            var paymentExistente = new PaymentInputDto(
                Guid.NewGuid(), orderId, 10m, DateTime.Now, null,
                (int)EPaymentMethod.QrCode, (int)EPaymentStatus.Pending,
                new byte[0], "qr.png", "path"
            );

            var orderDto = new OrderInputDto(
                orderId, DateTime.Now, (int)EStatusOrder.EmAberto, 10m, null, new List<ItemOrderInputDto>(),
                paymentExistente
            );

            _orderDataSourceMock.Setup(x => x.GetById(orderId)).ReturnsAsync(orderDto);

            Func<Task> act = async () => await _useCase.Run(orderId);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error:O pagamento deste pedido está sendo processado, favor aguardar.");
        }

        [Fact]
        public async Task Run_DeveLancarErro_QuandoFalhaGerarQrCode()
        {
            var orderId = Guid.NewGuid();
            var valor = 10m;

            var orderDto = new OrderInputDto(
                orderId, DateTime.Now, (int)EStatusOrder.EmAberto, valor, null, new List<ItemOrderInputDto>(), null
            );

            _orderDataSourceMock.Setup(x => x.GetById(orderId)).ReturnsAsync(orderDto);

            _paymentDataSourceMock.Setup(x => x.GenerateQrCodeAsync(orderId, valor))
                .ThrowsAsync(new Exception("Erro na API do Mercado Pago"));

            Func<Task> act = async () => await _useCase.Run(orderId);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error:Erro na API do Mercado Pago");
        }
    }
}