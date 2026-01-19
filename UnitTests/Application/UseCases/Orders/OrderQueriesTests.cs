using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.UseCases.Orders;
using Domain.Entities.Enums;
using FluentAssertions;
using Moq;
using Shared.DTO.Order.Input;
using Shared.DTO.Payment;

namespace Orders.UnitTests.Application.UseCases
{
    public class OrderQueriesTests
    {
        private readonly Mock<IOrderDataSource> _orderDataSourceMock;
        private readonly OrderGateway _orderGateway;

        public OrderQueriesTests()
        {
            _orderDataSourceMock = new Mock<IOrderDataSource>();
            _orderGateway = OrderGateway.Create(_orderDataSourceMock.Object);
        }


        [Fact]
        public async Task GetListOrders_DeveRetornarLista_ComSucesso()
        {

            var useCase = GetListOrdersUseCase.Create(_orderGateway);

            var listaDtos = new List<OrderInputDto>
            {
                new OrderInputDto(Guid.NewGuid(), DateTime.Now, (int)EStatusOrder.Recebido, 10m, null, new List<ItemOrderInputDto>(), null),
                new OrderInputDto(Guid.NewGuid(), DateTime.Now, (int)EStatusOrder.Pronto, 20m, null, new List<ItemOrderInputDto>(), null)
            };


            _orderDataSourceMock.Setup(x => x.GetListOrders())
                .ReturnsAsync(listaDtos);

            var result = await useCase.Run();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }


        [Fact]
        public async Task GetOrderById_DeveRetornarPedido_QuandoExiste()
        {
            var useCase = GetOrderByIdUseCase.Create(_orderGateway);
            var id = Guid.NewGuid();
            var dto = new OrderInputDto(id, DateTime.Now, (int)EStatusOrder.Recebido, 10m, null, new List<ItemOrderInputDto>(), null);

            _orderDataSourceMock.Setup(x => x.GetById(id))
                .ReturnsAsync(dto);

            var result = await useCase.Run(id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
        }


        [Fact]
        public async Task GetPaymentStatus_DeveRetornarPagamento_QuandoExiste()
        {
            var useCase = GetPaymentStatusOrderUseCase.Create(_orderGateway);
            var id = Guid.NewGuid();
            var paymentDto = new PaymentInputDto(Guid.NewGuid(), id, 50m, DateTime.Now, DateTime.Now, (int)EPaymentMethod.QrCode, (int)EPaymentStatus.Paid, new byte[0], "qr", "path");

            var orderDto = new OrderInputDto(id, DateTime.Now, (int)EStatusOrder.Recebido, 50m, null, new List<ItemOrderInputDto>(), paymentDto);

            _orderDataSourceMock.Setup(x => x.GetById(id))
                .ReturnsAsync(orderDto);

            var result = await useCase.Run(id);

            result.Should().NotBeNull();
            result!.PaymentStatus.Should().Be(EPaymentStatus.Paid);
        }

        [Fact]
        public async Task GetPaymentStatus_DeveLancarErro_QuandoPedidoNaoExiste()
        {
            var useCase = GetPaymentStatusOrderUseCase.Create(_orderGateway);
            _orderDataSourceMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync((OrderInputDto?)null);

            Func<Task> act = async () => await useCase.Run(Guid.NewGuid());

            await act.Should().ThrowAsync<Exception>().WithMessage("Error:Error: Order not find by Id.");
        }

        [Fact]
        public async Task GetPaymentStatus_DeveLancarErro_QuandoPedidoNaoTemPagamento()
        {
            var useCase = GetPaymentStatusOrderUseCase.Create(_orderGateway);
            var id = Guid.NewGuid();

            var orderDto = new OrderInputDto(id, DateTime.Now, (int)EStatusOrder.EmAberto, 50m, null, new List<ItemOrderInputDto>(), null);

            _orderDataSourceMock.Setup(x => x.GetById(id)).ReturnsAsync(orderDto);

            Func<Task> act = async () => await useCase.Run(id);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error:Este pedido ainda não tem informação de pagamento, favor realize o checkout para consultar as informações de pagamento.");
        }
    }
}