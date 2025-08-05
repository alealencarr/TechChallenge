using Domain.Entities.Aggregates.AggregateProduct;

namespace Domain.Entities
{
    public class Ingredient
    {
        public Ingredient(Guid id, DateTime createdAt, string name, decimal price)
        {
            Name = name;
            Id = id;
            Price = price;
            CreatedAt = createdAt;
        }
        public Ingredient(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Para criar um ingrediente é necessário informar o nome.");

            if (price == 0)
                throw new ArgumentNullException("Para criar um ingrediente o preço deve ser maior que zero.");

            Name = name;
            Id = Guid.NewGuid();
            Price = price;
            CreatedAt = DateTime.Now;
        }

        public DateTime CreatedAt { get; private set; }

        public Ingredient() { }
        public Guid Id { get; set; }

        public ICollection<ProductIngredient> ProductIngredients { get; private set; } = [];


        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
