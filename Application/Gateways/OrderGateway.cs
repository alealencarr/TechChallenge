using Application.Interfaces.DataSources;
using Domain.Entities;
using Domain.Entities.Aggregates.AggregateOrder;
using Domain.Entities.Enums;
using Shared.DTO.Order.Input;
using Shared.DTO.Payment;

namespace Application.Gateways
{
    public class OrderGateway
    {
        private IOrderDataSource _dataSource;

        private OrderGateway(IOrderDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        public static OrderGateway Create(IOrderDataSource dataSource)
        {
            return new OrderGateway(dataSource);
        }


        public async Task<Order?> GetById(Guid id)
        {
            var order = await _dataSource.GetById(id);

            return order is not null ? new Order(order.Id, order.CreatedAt, order.Itens.Select(x => new ItemOrder(x.Price,
                (x.IngredientsSnack is null ? new List<IngredientSnack>() :
                x.IngredientsSnack.Select(y => new IngredientSnack(y.Id, y.IngredientId, y.Additional, y.Price, y.ItemId, y.Quantity)).ToList())
                , x.Quantity, x.ProductId, x.OrderId, x.Id)).ToList(), order.CustomerId, (EStatusOrder)order.OrderStatus, order.Price, 
                (order.Payment is null? null : new Payment(order.Payment.Id, order.Payment.OrderId, order.Payment.Amount, order.Payment.QrBytes, order.Payment.CreatedAt, 
                (EPaymentStatus)order.Payment.PaymentStatus, (EPaymentMethod)order.Payment.PaymentMethod, order.Payment.FileName, order.Payment.PathRoot, order.Payment.PaidAt ) ) ): null;
        }

        public async Task<List<Order>> GetListOrders()
        {
            var orders = await _dataSource.GetListOrders();

            return orders.Select( order => new Order(order.Id, order.CreatedAt, order.Itens.Select(x => new ItemOrder(x.Price,
                (x.IngredientsSnack is null ? new List<IngredientSnack>() :
                x.IngredientsSnack.Select(y => new IngredientSnack(y.Id, y.IngredientId, y.Additional, y.Price, y.ItemId, y.Quantity)).ToList())
                , x.Quantity, x.ProductId, x.OrderId, x.Id)).ToList(), order.CustomerId, (EStatusOrder)order.OrderStatus, order.Price,
                (order.Payment is null ? null : new Payment(order.Payment.Id, order.Payment.OrderId, order.Payment.Amount, order.Payment.QrBytes, order.Payment.CreatedAt,
                (EPaymentStatus)order.Payment.PaymentStatus, (EPaymentMethod)order.Payment.PaymentMethod, order.Payment.FileName, order.Payment.PathRoot, order.Payment.PaidAt)))).ToList();
        }        

        public async Task UpdatePayment(Payment payment)
        {
            var paymentInput = new PaymentInputDto(payment.Id, payment.OrderId, payment.Amount, payment.CreatedAt, payment.PaidAt, (int)payment.PaymentMethod, (int)payment.PaymentStatus, payment.QrBytes, payment.FileName, payment.PathRoot);

            await _dataSource.UpdatePayment(paymentInput);   
        }

        public async Task UpdatePaymentAndStatusOrder(Payment payment, EStatusOrder status)
        {
            var paymentInput = new PaymentInputDto(payment.Id, payment.OrderId, payment.Amount, payment.CreatedAt, payment.PaidAt, (int)payment.PaymentMethod, (int)payment.PaymentStatus, payment.QrBytes, payment.FileName, payment.PathRoot);

            await _dataSource.UpdatePaymentAndStatusOrder(paymentInput, (int)status);
        }

        public async Task UpdateStatusOrder(Order order)
        {
            await _dataSource.UpdateStatusOrder(order.Id, (int)order.StatusOrder);
        }


        public async Task CreateOrder(Order order)
        {
            var orderInput = new OrderInputDto(order.Id, order.CreatedAt, (int)order.StatusOrder, order.Price, order.CustomerId, order.Itens.Select(x => new ItemOrderInputDto(x.Id, x.OrderId, x.ProductId, x.Quantity, x.Price,
                (x.Ingredients is not null ?
                x.Ingredients.Select(y => new IngredientSnackInputDto(y.Id, y.IdIngredient, y.ItemId, y.Additional, y.Quantity, y.Price)).ToList() : new List<IngredientSnackInputDto>()))).ToList(), null);

            await _dataSource.Create(orderInput);
        }


    }
}
