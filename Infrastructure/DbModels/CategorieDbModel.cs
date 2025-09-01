using Infrastructure.DbModels.ProductModelsAggregate;

namespace Infrastructure.DbModels
{
    public class CategorieDbModel
    {
        public CategorieDbModel() { }

        public CategorieDbModel(Guid id, string name, bool isEditavel, DateTime? createdAt = null)
        {
            Name = name;
            Id = id;
            CreatedAt = createdAt ?? DateTime.Now;
            IsEditavel = isEditavel;
        }

        public DateTime CreatedAt { get; private set; }
 
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsEditavel { get; set; } = false;
        public ICollection<ProductDbModel> Products { get; set; } = new List<ProductDbModel>();        
    }
}
