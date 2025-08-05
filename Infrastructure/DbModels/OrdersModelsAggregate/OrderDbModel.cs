using Domain.Entities;
using Infrastructure.DbModels.OrdersModelsAggregate;

namespace Infrastructure.DbModels
{
    public class OrderDbModel
    {
        public OrderDbModel(Guid? customerId, int statusPedido, Guid id, DateTime createdAt, List<ItemOrderDbModel> itens, decimal price, Guid? paymentId)
        {
            CustomerId = customerId;
            StatusPedido = statusPedido;
            Id = id;
            CreatedAt = createdAt;
            StatusPedido = statusPedido;
            Price = price;
            Itens = itens;
            PaymentId = paymentId;
        }
        protected OrderDbModel() { }

        public DateTime CreatedAt { get; private set; }

        public List<ItemOrderDbModel> Itens = new();

        public Guid? CustomerId { get; private set; }
        public Guid Id { get; private set; }
        public int StatusPedido { get;  set; }

        public CustomerDbModel? Customer { get; set; }

        public decimal Price { get; private set; } = 0M;

        // FK para o pagamento atual/último
        public Guid? PaymentId { get; set; }
        public PaymentDbModel? Payment { get; set; }

        // Todos os pagamentos da order
        public ICollection<PaymentDbModel>? Payments { get; set; } = new List<PaymentDbModel>();
    }
}


