using Shared.DTO.Categorie.Output;
using System.Text.Json.Serialization;

namespace Shared.DTO.Product.Output
{
    public record ProductOutputDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }

        public CategorieOutputDto? Categorie { get; set; }

        public string Description { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ProductIngredientOutputDto> Ingredients { get; set; } = [];

        public List<ProductImageOutputDto> Images { get; set; } = [];
    }
}
 