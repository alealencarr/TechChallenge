namespace Domain.Entities.Aggregates.AggregateProduct
{
    public class ProductIngredient
    {
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public Guid IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }
        public ProductIngredient(Guid productId, Guid ingredientId, int quantity)
        {
            ProductId = productId;
            IngredientId = ingredientId;
            Quantity = quantity;
        }

        public ProductIngredient(Guid ingredientId, int quantity)
        {
            IngredientId = ingredientId;
            Quantity = quantity;
        }

        protected ProductIngredient() { }
    }
}
