using Shared.DTO.Categorie.Output;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Shared.DTO.Product.Output
{
    [ExcludeFromCodeCoverage]
    public record ProductOutputDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }

        public Guid? CategorieId { get; set; }

        public string Description { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ProductIngredientOutputDto> Ingredients { get; set; } = [];

        public List<ProductImageOutputDto> Images { get; set; } = [];
    }
}
 