using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.UseCases.Orders;
using Domain.Entities.Aggregates.AggregateOrder;
using Domain.Entities.Enums;
using FluentAssertions;
using Moq;
using Shared.DTO.Order.Input;
using Shared.DTO.Payment;
using Shared.DTO.Product.Input;
using Xunit;

namespace Orders.UnitTests.Application.UseCases
{
    public class OrderCommandsTests
    {
        private readonly Mock<IOrderDataSource> _orderDataSourceMock;
        private readonly OrderGateway _orderGateway;

        public OrderCommandsTests()
        {
            _orderDataSourceMock = new Mock<IOrderDataSource>();
            _orderGateway = OrderGateway.Create(_orderDataSourceMock.Object);
        }


        [Fact]
        public async Task UpdatePayment_DeveAtualizarParaRecebido_QuandoPagamentoAprovado()
        {

            var useCase = UpdatePaymentOrderUseCase.Create(_orderGateway);
            var orderId = Guid.NewGuid();

            var paymentDto = new PaymentInputDto(Guid.NewGuid(), orderId, 10m, DateTime.Now, null, 1, (int)EPaymentStatus.Pending, new byte[0], "", "");
            var orderDto = new OrderInputDto(orderId, DateTime.Now, (int)EStatusOrder.EmAberto, 10m, null, new List<ItemOrderInputDto>(), paymentDto);

            _orderDataSourceMock.Setup(x => x.GetById(orderId)).ReturnsAsync(orderDto);


            _orderDataSourceMock.Setup(x => x.UpdatePaymentAndStatusOrder(It.IsAny<PaymentInputDto>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);


            var result = await useCase.Run(orderId, paymentDto.Id, EPaymentStatus.Paid, 10m);


            result.Should().NotBeNull();
            result!.PaymentStatus.Should().Be(EPaymentStatus.Paid);

            _orderDataSourceMock.Verify(x => x.UpdatePaymentAndStatusOrder(
                It.Is<PaymentInputDto>(p => p.PaymentStatus == (int)EPaymentStatus.Paid),
                (int)EStatusOrder.Recebido), Times.Once);
        }

        [Fact]
        public async Task UpdatePayment_DeveAtualizarParaCancelado_QuandoPagamentoRecusado()
        {

            var useCase = UpdatePaymentOrderUseCase.Create(_orderGateway);
            var orderId = Guid.NewGuid();

            var paymentDto = new PaymentInputDto(Guid.NewGuid(), orderId, 10m, DateTime.Now, null, 1, (int)EPaymentStatus.Pending, new byte[0], "", "");
            var orderDto = new OrderInputDto(orderId, DateTime.Now, (int)EStatusOrder.EmAberto, 10m, null, new List<ItemOrderInputDto>(), paymentDto);

            _orderDataSourceMock.Setup(x => x.GetById(orderId)).ReturnsAsync(orderDto);


            var result = await useCase.Run(orderId, paymentDto.Id, EPaymentStatus.Failed, 10m);


            _orderDataSourceMock.Verify(x => x.UpdatePaymentAndStatusOrder(
                It.IsAny<PaymentInputDto>(),
                (int)EStatusOrder.Cancelado), Times.Once);
        }

        [Fact]
        public async Task UpdatePayment_DeveLancarErro_SePedidoJaFoiRecebido()
        {

            var useCase = UpdatePaymentOrderUseCase.Create(_orderGateway);
            var orderId = Guid.NewGuid();
            var orderDto = new OrderInputDto(orderId, DateTime.Now, (int)EStatusOrder.Recebido, 10m, null, new List<ItemOrderInputDto>(), null);

            _orderDataSourceMock.Setup(x => x.GetById(orderId)).ReturnsAsync(orderDto);


            Func<Task> act = async () => await useCase.Run(orderId, Guid.NewGuid(), EPaymentStatus.Paid, 10m);


            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error:Este pedido já foi pago e recebido pela cozinha.");
        }



        [Fact]
        public async Task UpdateStatus_DeveAvancarStatus_QuandoValido()
        {

            var useCase = UpdateStatusOrderUseCase.Create(_orderGateway);
            var orderId = Guid.NewGuid();


            var orderDto = new OrderInputDto(orderId, DateTime.Now, (int)EStatusOrder.Recebido, 10m, null, new List<ItemOrderInputDto>(), null);

            _orderDataSourceMock.Setup(x => x.GetById(orderId)).ReturnsAsync(orderDto);


            _orderDataSourceMock.Setup(x => x.UpdateStatusOrder(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);


            var result = await useCase.Run(orderId);


            result.Should().NotBeNull();
            result!.StatusOrder.Should().Be(EStatusOrder.EmPreparacao);


            _orderDataSourceMock.Verify(x => x.UpdateStatusOrder(orderId, (int)EStatusOrder.EmPreparacao), Times.Once);
        }

        [Fact]
        public async Task UpdateStatus_DeveLancarErro_SeStatusEmAberto()
        {

            var useCase = UpdateStatusOrderUseCase.Create(_orderGateway);
            var orderId = Guid.NewGuid();
            var orderDto = new OrderInputDto(orderId, DateTime.Now, (int)EStatusOrder.EmAberto, 10m, null, new List<ItemOrderInputDto>(), null);

            _orderDataSourceMock.Setup(x => x.GetById(orderId)).ReturnsAsync(orderDto);


            Func<Task> act = async () => await useCase.Run(orderId);


            await act.Should().ThrowAsync<Exception>().WithMessage("Error:É necessário primeiro fazer o checkout do pedido.");
        }

        [Fact]
        public async Task UpdateStatus_DeveLancarErro_SeStatusCancelado()
        {

            var useCase = UpdateStatusOrderUseCase.Create(_orderGateway);
            var orderId = Guid.NewGuid();
            var orderDto = new OrderInputDto(orderId, DateTime.Now, (int)EStatusOrder.Cancelado, 10m, null, new List<ItemOrderInputDto>(), null);

            _orderDataSourceMock.Setup(x => x.GetById(orderId)).ReturnsAsync(orderDto);


            Func<Task> act = async () => await useCase.Run(orderId);


            await act.Should().ThrowAsync<Exception>().WithMessage("Error:Este pedido teve o pagamento recusado e foi cancelado. Não é possivel realizar alterações, nem gerar um novo pagamento para ele.");
        }

        [Fact]
        public async Task UpdateStatus_DeveLancarErro_SeStatusFinalizado()
        {

            var useCase = UpdateStatusOrderUseCase.Create(_orderGateway);
            var orderId = Guid.NewGuid();
            var orderDto = new OrderInputDto(orderId, DateTime.Now, (int)EStatusOrder.Finalizado, 10m, null, new List<ItemOrderInputDto>(), null);

            _orderDataSourceMock.Setup(x => x.GetById(orderId)).ReturnsAsync(orderDto);


            Func<Task> act = async () => await useCase.Run(orderId);


            await act.Should().ThrowAsync<Exception>().WithMessage("Error:Este pedido já foi finalizado.");
        }
    }
}