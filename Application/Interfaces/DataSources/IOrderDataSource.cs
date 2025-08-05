using Shared.DTO.Order.Input;
using Shared.DTO.Payment;

namespace Application.Interfaces.DataSources
{
    public interface IOrderDataSource
    {
        Task Create(OrderInputDto order);
        Task<OrderInputDto> GetById(Guid id);

        Task<List<OrderInputDto>> GetListOrders();
        Task UpdatePayment(PaymentInputDto payment);
        Task UpdateStatusOrder(Guid id, int statusOrder);
        Task UpdatePaymentAndStatusOrder(PaymentInputDto payment, int statusOrder);
    }
}
