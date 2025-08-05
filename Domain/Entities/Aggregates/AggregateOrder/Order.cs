using Domain.Entities.Enums;

namespace Domain.Entities.Aggregates.AggregateOrder
{
    public class Order
    {

        private Order(Guid? customerId, List<ItemOrder> items)
        {
            if (!items.Any()) //Verifica se tem algum item                  
                throw new ArgumentException($"Para criar um pedido é necessário pelo menos 1 item.");

            CustomerId = customerId;
            StatusOrder = Enums.EStatusOrder.EmAberto;
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;

            foreach (var item in items)
            {
                AdicionarItem(item);
            }
        }

        public static Order Create(Guid? clientId, List<ItemOrder> items)
        {
            return new Order(clientId, items);
        }

        public Order(Guid id, DateTime createdAt, List<ItemOrder> items, Guid? customerId, EStatusOrder statusOrder, decimal price, Payment? payment)
        {
            Id = id;
            CreatedAt = createdAt;
            _itens = items;
            Price = price;
            CustomerId = customerId;
            StatusOrder = statusOrder;
            Payment = payment;
        }

        public DateTime CreatedAt { get; private set; }

        private readonly List<ItemOrder> _itens = new();
        public IReadOnlyCollection<ItemOrder> Itens => _itens;

        public Guid? CustomerId { get; private set; }
        public Guid Id { get; private set; }
        public Enums.EStatusOrder StatusOrder { get; private set; }

        public Customer? Customer { get; set; }

        public Guid? PaymentId { get; set; }

        public Payment? Payment { get; set; }

        public decimal Price { get; private set; } = 0M;
        public void AdicionarItem(ItemOrder itemBase)
        {
            itemBase.OrderId = Id;
            Price += itemBase.GetPrice();

            _itens.Add(itemBase);
        }

        public bool UpdateStatus()
        {
            switch (StatusOrder)
            {
                case Enums.EStatusOrder.EmAberto:
                    StatusOrder = Enums.EStatusOrder.Recebido;
                    return true;

                case Enums.EStatusOrder.Recebido:
                    StatusOrder = Enums.EStatusOrder.EmPreparacao;
                    return true;

                case Enums.EStatusOrder.EmPreparacao:
                    StatusOrder = Enums.EStatusOrder.Pronto;
                    return true;

                case Enums.EStatusOrder.Pronto:
                    StatusOrder = Enums.EStatusOrder.Finalizado;
                    return true;

                case Enums.EStatusOrder.Finalizado:
                    throw new Exception("Este pedido já foi finalizado.");

                default:
                    throw new Exception("Status do pedido inválido.");                    
            }
        }

    }
}
