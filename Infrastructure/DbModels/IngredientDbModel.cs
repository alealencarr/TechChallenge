using Infrastructure.DbModels.ProductModelsAggregate;

namespace Infrastructure.DbModels;

public class IngredientDbModel
{
    public IngredientDbModel(Guid id, string name, decimal price, DateTime? createdAt = null)
    {
        Name = name;
        Id = id;
        Price = price;
        CreatedAt = createdAt ?? DateTime.Now;
    }

    public DateTime CreatedAt { get; private set; }

    public IngredientDbModel() { }
    public Guid Id { get; set; }

    public ICollection<ProductIngredientDbModel> ProductIngredients { get; private set; } = [];


    public string Name { get; set; }
    public decimal Price { get; set; }
}
