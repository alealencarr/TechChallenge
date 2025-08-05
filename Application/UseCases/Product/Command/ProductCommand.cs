using Shared.DTO.Product.Request;

namespace Application.UseCases.Products.Command;

public class ProductCommand
{
    public ProductCommand(Guid id, string name, decimal price, Guid categorieId, string description, List<ProductImageRequestDto>? images, List<ProductIngredientRequestDto>? ingredients)
    {
        Id = id;
        this.Constructor(name, price, categorieId, description, images, ingredients);
    }

    public ProductCommand(string name, decimal price, Guid categorieId, string description, List<ProductImageRequestDto>? images, List<ProductIngredientRequestDto>? ingredients)
    {
        this.Constructor(name, price, categorieId, description, images, ingredients);
    }

    private void Constructor(string name, decimal price, Guid categorieId, string description, List<ProductImageRequestDto>? images, List<ProductIngredientRequestDto>? ingredients)
    {
        Name = name;
        Price = price;
        CategorieId = categorieId;
        Description = description;
        Images = images;
        Ingredients = ingredients;
    }

    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public Guid Id { get; private set; }

    public Guid CategorieId { get; private set; }
    public string Description { get; private set; }
    public List<ProductImageRequestDto>? Images { get; private set; }
    public List<ProductIngredientRequestDto>? Ingredients { get; private set; }
}



