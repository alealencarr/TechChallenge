using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Application.Presenter.Orders;
using Application.UseCases.Orders;
using Application.UseCases.Orders.Command;
using Domain.Entities.Enums;
using Shared.DTO.Order.Output.CheckoutOrder;
using Shared.DTO.Order.Output.OrderCompleted;
using Shared.DTO.Order.Output.OrderSummary;
using Shared.DTO.Order.Request;
using Shared.DTO.Payment;
using Shared.Result;

namespace Application.Controllers.Orders
{
    public class OrderController
    {
        private IOrderDataSource _dataSource;
        private IProductDataSource? _dataSourceProduct;
        private IIngredientDataSource? _dataSourceIngredient;
        private ICustomerDataSource? _dataSourceCustomer;
        private IPaymentDataSource? _dataSourcePayment;
        private IFileStorageService? _fileStorageService;

        public OrderController(IOrderDataSource dataSource, IIngredientDataSource? ingredientDataSource, ICustomerDataSource? customerDataSource, IProductDataSource? productDataSource)
        {
            _dataSource = dataSource;
            _dataSourceCustomer = customerDataSource;
            _dataSourceIngredient = ingredientDataSource;
            _dataSourceProduct = productDataSource;
        }

        public OrderController(IOrderDataSource dataSource, IPaymentDataSource? paymentDataSource, IFileStorageService fileStorageService)
        {
            _dataSource = dataSource;
            _dataSourcePayment = paymentDataSource;
            _fileStorageService = fileStorageService;
        }
        public OrderController(IOrderDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        public async Task<ICommandResult<List<OrderSummaryOutputDto>?>> GetListOrders()
        {
            OrderPresenter orderPresenter = new("Pedidos encontrados!");

            try
            {
                var orderGateway = OrderGateway.Create(_dataSource);
                var useCase = GetListOrdersUseCase.Create(orderGateway);
                var orders = await useCase.Run();

                return orders is null ? orderPresenter.Error<List<OrderSummaryOutputDto>?>("Orders not found.") : orderPresenter.TransformListSummary(orders);
            }
            catch (Exception ex)
            {
                return orderPresenter.Error<List<OrderSummaryOutputDto>?>(ex.Message);
            }
        }

        
        public async Task<ICommandResult<OrderSummaryOutputDto?>> CreateOrder(OrderRequestDto orderRequestDto)
        {
            OrderPresenter orderPresenter = new("Pedido criado!");

            try
            {
                var command = new OrderCommand(orderRequestDto.CustomerId, orderRequestDto.Itens);

                var orderGateway = OrderGateway.Create(_dataSource);
                var ingredientGateway = IngredientGateway.Create(_dataSourceIngredient!);
                var customerGateway = CustomerGateway.Create(_dataSourceCustomer!);
                var productGateway = ProductGateway.Create(_dataSourceProduct!);

                var useCaseCreate = CreateOrderUseCase.Create(orderGateway, customerGateway, ingredientGateway, productGateway);
                var orderEntity = await useCaseCreate.Run(command);

                var dtoRetorno = orderPresenter.TransformObjectSummary(orderEntity);

                return dtoRetorno;
            }
            catch (Exception ex)
            {
                return orderPresenter.Error<OrderSummaryOutputDto?>(ex.Message);
            }
        }

        public async Task<ICommandResult<OrderOutputDto?>> GetOrderById(Guid id)
        {
            OrderPresenter orderPresenter = new("Pedido encontrado!");

            try
            {
                var orderGateway = OrderGateway.Create(_dataSource);
                var useCase = GetOrderByIdUseCase.Create(orderGateway);
                var order = await useCase.Run(id);

                return order is null ? orderPresenter.Error<OrderOutputDto?>("Order not found.") : orderPresenter.TransformObject(order);
            }
            catch (Exception ex)
            {
                return orderPresenter.Error<OrderOutputDto?>(ex.Message);
            }
        }

        public async Task<ICommandResult<PaymentOutputDto?>> GetPaymentStatusOrder(Guid id)
        {
            OrderPresenter orderPresenter = new("Status de pagamento encontrado!");

            try
            {
                var orderGateway = OrderGateway.Create(_dataSource);
                var useCase = GetPaymentStatusOrderUseCase.Create(orderGateway);
                var order = await useCase.Run(id);

                return order is null ? orderPresenter.Error<PaymentOutputDto?>("Order not found.") : orderPresenter.TransformObjectPaymentStatus(order);
            }
            catch (Exception ex)
            {
                return orderPresenter.Error<PaymentOutputDto?>(ex.Message);
            }
        }

        
        public async Task<ICommandResult<OrderSummaryOutputDto?>> UpdateStatusOrder(Guid id)
        {
            OrderPresenter orderPresenter = new("Status do pedido foi atualizado!");

            try
            {
                var orderGateway = OrderGateway.Create(_dataSource);
                var useCase = UpdateStatusOrderUseCase.Create(orderGateway);
                var order = await useCase.Run(id);

                return order is null ? orderPresenter.Error<OrderSummaryOutputDto?>("Order not found.") : orderPresenter.TransformObjectSummary(order);
            }
            catch (Exception ex)
            {
                return orderPresenter.Error<OrderSummaryOutputDto?>(ex.Message);
            }
        }

        public async Task<ICommandResult<QrCodeOrderOutputDto?>> CheckoutOrder(Guid id)
        {
            OrderPresenter orderPresenter = new("Checkout realizado!");
            try
            {
                var orderGateway = OrderGateway.Create(_dataSource);
                var paymentGateway = PaymentGateway.Create(_dataSourcePayment);
                var useCase = CheckoutOrderUseCase.Create(orderGateway, paymentGateway, _fileStorageService);
                var payment = await useCase.Run(id);

                return payment is null ? orderPresenter.Error<QrCodeOrderOutputDto?>("Order not found.") : orderPresenter.TransformObjectPayment(payment);
            }
            catch (Exception ex)
            {
                return orderPresenter.Error<QrCodeOrderOutputDto?>(ex.Message);
            }
        }

        public async Task<ICommandResult> UpdatePaymentOrder(PaymentNotificationDto? paymentDto)
        {
            OrderPresenter orderPresenter = new("Evento de pagamento recebido!");
            try
            {
                var orderGateway = OrderGateway.Create(_dataSource);
                var useCase = UpdatePaymentOrderUseCase.Create(orderGateway);
                var payment = await useCase.Run(paymentDto.OrderId, paymentDto.Id, (EPaymentStatus)paymentDto.Status, paymentDto.Amount);

                return orderPresenter.Success();
            }
            catch (Exception ex)
            {
                return orderPresenter.Success();
            }
        }

    }
}
