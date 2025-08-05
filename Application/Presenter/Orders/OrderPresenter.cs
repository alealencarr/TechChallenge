using Application.Common;
using Domain.Entities;
using Domain.Entities.Aggregates.AggregateOrder;
using Shared.DTO.Order.Output;
using Shared.DTO.Order.Output.CheckoutOrder;
using Shared.DTO.Order.Output.OrderCompleted;
using Shared.DTO.Order.Output.OrderSummary;
using Shared.DTO.Payment;
using Shared.Result;

namespace Application.Presenter.Orders
{
    public class OrderPresenter
    {
        private string _message;
        public OrderPresenter(string? message = null) { _message = message ?? string.Empty; }

        public ICommandResult RetornoSucess()
        {
            return CommandResult.Success(_message);
        }
        public ICommandResult<T> Error<T>(string message)
        {
            return CommandResult<T>.Fail(message);
        }

        public ICommandResult Error(string message)
        {
            return CommandResult.Fail(message);
        }

        public ICommandResult Success()
        {
            return CommandResult.Success();
        }

        #region Summary
        public ICommandResult<List<OrderSummaryOutputDto>> TransformListSummary(List<Order> order)
        {
            return CommandResult<List<OrderSummaryOutputDto>>.Success(order.Select(x => TransformSummary(x)).ToList(), _message);
        }


        public ICommandResult<OrderSummaryOutputDto> TransformObjectSummary(Order order)
        {
            return CommandResult<OrderSummaryOutputDto>.Success(TransformSummary(order), _message);
        }

        public OrderSummaryOutputDto TransformSummary(Order order)
        {
            return new OrderSummaryOutputDto
            {
                Id = order.Id,
                Price = order.Price,
                OrderStatus = new Shared.DTO.Order.Output.OrderStatusOutputDto { Id = (int)order.StatusOrder, Description = order.StatusOrder.ToString() },
                CreatedAt = order.CreatedAt
            };
        }

        #endregion

        #region Payment
        public ICommandResult<PaymentOutputDto> TransformObjectPaymentStatus(Payment order)
        {
            return CommandResult<PaymentOutputDto>.Success(TransformPaymentStatus(order), _message);
        }

        public PaymentOutputDto TransformPaymentStatus(Payment payment)
        {
            return new PaymentOutputDto(payment.Id, payment.OrderId, payment.Amount, new MethodPaymentDto((int)payment.PaymentMethod, payment.PaymentMethod.ToString()),
                new StatusPaymentDto((int)payment.PaymentStatus, payment.PaymentStatus.ToString()));

        }
        #endregion

        #region QrCode
        public QrCodeOrderOutputDto TransformPayment(Payment payment)
        {
            return new QrCodeOrderOutputDto
            (
                  payment.OrderId,
                 $"/qrcodes/{payment.FileName}".ToAbsoluteUrl()
            );
        }

        public ICommandResult<QrCodeOrderOutputDto> TransformObjectPayment(Payment payment)
        {
            return CommandResult<QrCodeOrderOutputDto>.Success(TransformPayment(payment), _message);
        }

        #endregion

        #region OrderCompleted
        public OrderOutputDto Transform(Order order)
        {
            return new OrderOutputDto
            {
                Id = order.Id,
                Price = order.Price,
                CreatedAt = order.CreatedAt,
                CustomerId = order.CustomerId,
                OrderStatus = new OrderStatusOutputDto { Description = order.StatusOrder.ToString(), Id = (int)order.StatusOrder },
                Itens = order.Itens.Select(x => new ItemOrderOutputDto
                {
                    Quantity = x.Quantity,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    Ingredients = x.Ingredients is not null ?
                    x.Ingredients.Select(y => new IngredientItemOrderOutputDto { Quantity = y.Quantity, Price = y.Price, Additional = y.Additional, IngredientId = y.IdIngredient }).ToList() :
                    new List<IngredientItemOrderOutputDto>()
                }).ToList()
            };
        }

        public ICommandResult<OrderOutputDto> TransformObject(Order order)
        {
            return CommandResult<OrderOutputDto>.Success(Transform(order), _message);
        }
        #endregion


    }
}
