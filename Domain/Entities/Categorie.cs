using Domain.Entities.Aggregates.AggregateProduct;

namespace Domain.Entities
{
    public class Categorie
    {
        public Categorie(string name, Guid id, DateTime createdAt)
        {
            Name = name;
            Id = id;
            CreatedAt = createdAt;
        }
        public Categorie(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }

        public DateTime CreatedAt { get; private set; }
        public Categorie() { }
 
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<Product> Produtos { get; set; } = new List<Product>();

        public bool IsLanche()
        {
            return Name.Equals("Lanche", StringComparison.OrdinalIgnoreCase);
        }
    }
}
