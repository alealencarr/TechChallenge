using Domain.Entities;
using Infrastructure.DbModels.ProductModelsAggregate;

namespace Infrastructure.DbModels
{
    public class ProductDbModel
    {
        protected ProductDbModel() { }

        public ProductDbModel(Guid id, string name, decimal price, Guid categorieId, string description, DateTime createdAt, List<ProductImageDbModel> productImages, List<ProductIngredientDbModel> productIngredients )
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            CategorieId = categorieId;
            CreatedAt = createdAt;
            ProductImages = productImages;
            ProductIngredients = productIngredients;
        }

        public DateTime CreatedAt { get; set; }
        public Guid Id { get; private set; }
        public string Name { get; set; }

        public decimal Price { get; set; }
        public Guid CategorieId { get; set; }
        public CategorieDbModel? Categorie { get; set; }

        public string Description { get; set; }

        public List<ProductImageDbModel> ProductImages { get;  set; } = [];

        public List<ProductIngredientDbModel> ProductIngredients { get;  set; } = []; 
    }
}
