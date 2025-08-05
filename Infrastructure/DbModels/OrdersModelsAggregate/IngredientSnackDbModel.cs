
namespace Infrastructure.DbModels.OrdersModelsAggregate
{
    public class IngredientSnackDbModel
    {
        public Guid Id { get; private set; }
        public Guid IdIngredient { get; set; }
        public decimal Price { get; set; } = 0M;
        public bool Additional { get; private set; }
        public Guid ItemId { get; set; }

        public int Quantity { get; set; }

        public ItemOrderDbModel ItemOrder { get; set; }  // <- relação com o item do pedido

        public IngredientSnackDbModel(Guid id, Guid idIngredient, bool additional, decimal price, Guid itemId, int quantity)
        {
            Id = id;
            IdIngredient = idIngredient;
            Additional = additional;
            Price = price;
            ItemId = itemId;
            Quantity = quantity;
        }

        protected IngredientSnackDbModel() { }
    }
}
