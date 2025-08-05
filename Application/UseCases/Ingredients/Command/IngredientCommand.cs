namespace Application.UseCases.Ingredients.Command;

public class IngredientCommand
{
    public IngredientCommand(Guid id,  string name, decimal price)
    {
        Name = name;
        Price = price;
        Id = id;
    }

    public IngredientCommand(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public string Name { get; private set; } 
    public decimal Price { get; private set; } 
    public Guid Id { get; private set; }
}