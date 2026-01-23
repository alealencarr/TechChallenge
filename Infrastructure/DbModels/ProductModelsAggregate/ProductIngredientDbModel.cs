using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.DbModels.ProductModelsAggregate
{
    [ExcludeFromCodeCoverage]
    public class ProductIngredientDbModel
    {
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public ProductDbModel? Product { get; set; }
        public Guid IngredientId { get; set; }
        public ProductIngredientDbModel(Guid productId, Guid ingredientId, int quantity)
        {
            ProductId = productId;
            IngredientId = ingredientId;
            Quantity = quantity;
        }

        protected ProductIngredientDbModel() { }
    }
}
