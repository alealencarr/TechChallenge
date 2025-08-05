using Domain.Entities.Aggregates.AggregateOrder;

namespace Infrastructure.DbModels.OrdersModelsAggregate
{
    public class ItemOrderDbModel
    {
        protected ItemOrderDbModel() { }
        public ItemOrderDbModel(Guid orderId, Guid productId, decimal price, int quantity, List<IngredientSnackDbModel>? ingredientsSnack)
        {
            OrderId = orderId;
            Id = Guid.NewGuid();
            Price = price * quantity;
            ProductId = productId;
            Quantity = quantity;
            IngredientsSnack = ingredientsSnack;
        }
        public decimal Price { get; private set; } = 0M;

        public List<IngredientSnackDbModel>? IngredientsSnack = new();

        public int Quantity { get; set; }
        public Guid ProductId { get; private set; }
        public Guid OrderId { get; private set; }
        public OrderDbModel? Order { get; set; }
        public Guid Id { get; private set; }

    }

}


