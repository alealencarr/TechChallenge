using Application.Controllers.Orders;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Domain.Entities.Enums;
using FluentAssertions;
using Moq;
using Shared.DTO.Categorie.Input;
using Shared.DTO.Order.Input;
using Shared.DTO.Order.Request;
using Shared.DTO.Payment;
using Shared.DTO.Product.Input;

namespace Orders.UnitTests.Application.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderDataSource> _orderDataSourceMock;
        private readonly Mock<ICustomerDataSource> _customerDataSourceMock;
        private readonly Mock<IProductDataSource> _productDataSourceMock;
        private readonly Mock<IIngredientDataSource> _ingredientDataSourceMock;
        private readonly Mock<IPaymentDataSource> _paymentDataSourceMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;

        public OrderControllerTests()
        {
            _orderDataSourceMock = new Mock<IOrderDataSource>();
            _customerDataSourceMock = new Mock<ICustomerDataSource>();
            _productDataSourceMock = new Mock<IProductDataSource>();
            _ingredientDataSourceMock = new Mock<IIngredientDataSource>();
            _paymentDataSourceMock = new Mock<IPaymentDataSource>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();
        }


        [Fact]
        public async Task GetListOrders_DeveRetornarSucesso_QuandoExistemPedidos()
        {
            var controller = new OrderController(_orderDataSourceMock.Object);

            var ordersDto = new List<OrderInputDto>
            {
                new OrderInputDto(Guid.NewGuid(), DateTime.Now, (int)EStatusOrder.Pronto, 50m, null, new List<ItemOrderInputDto>(), null)
            };

            _orderDataSourceMock.Setup(x => x.GetListOrders()).ReturnsAsync(ordersDto);

            var result = await controller.GetListOrders();

            result.Succeeded.Should().BeTrue();
            result.Messages[0].Should().Be("Pedidos encontrados!");
            result.Data.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetOrderById_DeveRetornarSucesso_QuandoEncontrado()
        {
            var controller = new OrderController(_orderDataSourceMock.Object);
            var id = Guid.NewGuid();
            var orderDto = new OrderInputDto(id, DateTime.Now, (int)EStatusOrder.Recebido, 100m, null, new List<ItemOrderInputDto>(), null);

            _orderDataSourceMock.Setup(x => x.GetById(id)).ReturnsAsync(orderDto);

            var result = await controller.GetOrderById(id);

            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetOrderById_DeveRetornarErro_QuandoNaoEncontrado()
        {
            var controller = new OrderController(_orderDataSourceMock.Object);
            var id = Guid.NewGuid();

            _orderDataSourceMock.Setup(x => x.GetById(id)).ReturnsAsync((OrderInputDto?)null);

            var result = await controller.GetOrderById(id);

            result.Succeeded.Should().BeFalse();
            result.Messages[0].Should().Be("Order not found.");
        }


        //[Fact]
        //public async Task CreateOrder_DeveCriarPedido_ComSucesso()
        //{
        //    var controller = new OrderController(
        //        _orderDataSourceMock.Object,
        //        _ingredientDataSourceMock.Object,
        //        _customerDataSourceMock.Object,
        //        _productDataSourceMock.Object
        //    );

        //    var customerId = Guid.NewGuid();
        //    var productId = Guid.NewGuid();
        //    var request = new OrderRequestDto
        //    {
        //        CustomerId = customerId,
        //        Itens = new List<ItemOrderRequestDto>
        //        {
        //            new ItemOrderRequestDto { Id = productId, Quantity = 1, IngredientsSnack = new List<IngredientSnackRequestDto>() }
        //        }
        //    };

        //    _customerDataSourceMock.Setup(x => x.GetById(customerId))
        //        .ReturnsAsync(new CustomerDto(customerId, DateTime.Now, "111", "Nome", "email", true));

        //    _productDataSourceMock.Setup(x => x.GetByIds(It.IsAny<List<Guid>>()))
        //        .ReturnsAsync(new List<ProductInputDto> {
        //            new ProductInputDto(productId, DateTime.Now, "Lanche", "Desc", 20m, Guid.NewGuid(), new List<ProductImageInputDto>(), new List<ProductIngredientInputDto>(), true)
        //        });

        //    _orderDataSourceMock.Setup(x => x.Create(It.IsAny<OrderInputDto>()))
        //        .Returns(Task.CompletedTask);

        //    var result = await controller.CreateOrder(request);

        //    result.Succeeded.Should().BeTrue();
        //    result.Messages[0].Should().Be("Pedido criado!");
        //    result.Data.Should().NotBeNull();
        //    result.Data.Price.Should().Be(20m);
        //}

        //[Fact]
        //public async Task CreateOrder_DeveRetornarErro_QuandoValidacaoFalha()
        //{
        //    var controller = new OrderController(
        //        _orderDataSourceMock.Object, _ingredientDataSourceMock.Object, _customerDataSourceMock.Object, _productDataSourceMock.Object
        //    );

        //    var request = new OrderRequestDto
        //    {
        //        Itens = new List<ItemOrderRequestDto> { new ItemOrderRequestDto { Id = Guid.NewGuid() } }
        //    };

        //    _productDataSourceMock.Setup(x => x.GetByIds(It.IsAny<List<Guid>>())).ReturnsAsync(new List<ProductInputDto>());

        //    var result = await controller.CreateOrder(request);

        //    result.Succeeded.Should().BeFalse();
        //    result.Messages[0].Should().Contain("Os seguintes produtos não foram encontrados");
        //}


        //[Fact]
        //public async Task CheckoutOrder_DeveGerarQrCode_E_AtualizarPagamento()
        //{
        //    var controller = new OrderController(
        //        _orderDataSourceMock.Object,
        //        _paymentDataSourceMock.Object,
        //        _fileStorageServiceMock.Object
        //    );

        //    var orderId = Guid.NewGuid();
        //    var valor = 50m;
        //    var qrBytes = new byte[] { 1, 2 };

        //    var orderDto = new OrderInputDto(orderId, DateTime.Now, (int)EStatusOrder.EmAberto, valor, null, new List<ItemOrderInputDto>(), null);

        //    _orderDataSourceMock.Setup(x => x.GetById(orderId)).ReturnsAsync(orderDto);

        //    _paymentDataSourceMock.Setup(x => x.GenerateQrCodeAsync(orderId, valor)).ReturnsAsync(qrBytes);

        //    _fileStorageServiceMock.Setup(x => x.SaveFileAsync(qrBytes, It.IsAny<string>(), It.IsAny<string>()))
        //        .ReturnsAsync("path/qrcode.png");

        //    _orderDataSourceMock.Setup(x => x.UpdatePayment(It.IsAny<PaymentInputDto>())).Returns(Task.CompletedTask);

        //    _orderDataSourceMock.Setup(x => x.UpdatePaymentAndStatusOrder(It.IsAny<PaymentInputDto>(), It.IsAny<int>())).Returns(Task.CompletedTask);


        //    var result = await controller.CheckoutOrder(orderId);

        //    result.Succeeded.Should().BeTrue();
        //    result.Messages[0].Should().Be("Checkout realizado!");
        //    result.Data.Should().NotBeNull();

        //}


        [Fact]
        public async Task UpdateStatusOrder_DeveAtualizarStatus()
        {
            var controller = new OrderController(_orderDataSourceMock.Object);
            var id = Guid.NewGuid();

            var orderDto = new OrderInputDto(id, DateTime.Now, (int)EStatusOrder.Recebido, 100m, null, new List<ItemOrderInputDto>(), null);

            _orderDataSourceMock.Setup(x => x.GetById(id)).ReturnsAsync(orderDto);
            _orderDataSourceMock.Setup(x => x.UpdateStatusOrder(id, (int)EStatusOrder.EmPreparacao)).Returns(Task.CompletedTask);

            var result = await controller.UpdateStatusOrder(id);

            result.Succeeded.Should().BeTrue();
            result.Messages[0].Should().Be("Status do pedido foi atualizado!");
            result.Data.OrderStatus.Description.Should().Be(EStatusOrder.EmPreparacao.ToString());
        }

        [Fact]
        public async Task UpdatePaymentOrder_Webhook_DeveProcessarComSucesso()
        {
            var controller = new OrderController(_orderDataSourceMock.Object);

            var notification = new PaymentNotificationDto
            (
                 Guid.NewGuid(),
                 Guid.NewGuid(),
                 (int)EPaymentStatus.Paid,
                 50m
            );

            var paymentDto = new PaymentInputDto(notification.Id, notification.OrderId, 50m, DateTime.Now, null, 1, (int)EPaymentStatus.Pending, new byte[0], "", "");
            var orderDto = new OrderInputDto(notification.OrderId, DateTime.Now, (int)EStatusOrder.EmAberto, 50m, null, new List<ItemOrderInputDto>(), paymentDto);

            _orderDataSourceMock.Setup(x => x.GetById(notification.OrderId)).ReturnsAsync(orderDto);

            _orderDataSourceMock.Setup(x => x.UpdatePaymentAndStatusOrder(It.IsAny<PaymentInputDto>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var result = await controller.UpdatePaymentOrder(notification);

            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task GetPaymentStatusOrder_DeveRetornarStatus()
        {

            var controller = new OrderController(_orderDataSourceMock.Object);
            var id = Guid.NewGuid();
            var payment = new PaymentInputDto(Guid.NewGuid(), id, 10m, DateTime.Now, null, 1, (int)EPaymentStatus.Paid, new byte[0], "", "");
            var orderDto = new OrderInputDto(id, DateTime.Now, (int)EStatusOrder.Recebido, 10m, null, new List<ItemOrderInputDto>(), payment);

            _orderDataSourceMock.Setup(x => x.GetById(id)).ReturnsAsync(orderDto);


            var result = await controller.GetPaymentStatusOrder(id);


            result.Succeeded.Should().BeTrue();
            result.Data.PaymentStatus.Description.Should().Be(EPaymentStatus.Paid.ToString());
        }
    }
}