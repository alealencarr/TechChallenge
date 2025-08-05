namespace Infrastructure.DbModels.ProductModelsAggregate
{
    public class ProductIngredientDbModel
    {
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public ProductDbModel? Product { get; set; }
        public Guid IngredientId { get; set; }
        public IngredientDbModel? Ingredient { get; set; }
        public ProductIngredientDbModel(Guid productId, Guid ingredientId, int quantity)
        {
            ProductId = productId;
            IngredientId = ingredientId;
            Quantity = quantity;
        }

        protected ProductIngredientDbModel() { }
    }
}
