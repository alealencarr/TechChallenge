namespace Domain.Entities.Aggregates.AggregateOrder
{
    public class IngredientSnack
    {

        public ICollection<ItemOrder> ItensPedido { get; set; } = new List<ItemOrder>();

        public Guid Id { get; private set; }
        public Guid IdIngredient { get; set; }
        public decimal Price { get; set; } = 0M;
        public bool Additional { get; private set; }
        public Guid ItemId { get; set; }

        public int Quantity { get; set; }
        public IngredientSnack(Guid id, Guid idIngrediente, bool additional, decimal price, Guid itemId, int quantity)
        {
            Id = id;
            IdIngredient = idIngrediente;
            Additional = additional;
            Price = price;
            ItemId = itemId;
            Quantity = quantity;
        }

        public IngredientSnack(Guid idIngrediente, int quantity, Ingredient ingredient)
        {
            Id = Guid.NewGuid();
            Price = ingredient.Price;
            IdIngredient = idIngrediente;
            Quantity = quantity;
        }

        public IngredientSnack(Guid idIngrediente, bool additional, int quantity, decimal price, Guid itemId)
        {
            Id = Guid.NewGuid();
            Additional = additional;
            Price = price;
            IdIngredient = idIngrediente;
            ItemId = itemId;
            Quantity = quantity;
        }

        protected IngredientSnack() { }
    }
}
