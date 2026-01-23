using Shared.DTO.Categorie.Input;
using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Product.Input;
[ExcludeFromCodeCoverage]
public record ProductInputDto(Guid Id, DateTime CreatedAt, string Name, string Description, decimal Price, Guid CategorieId, ICollection<ProductImageInputDto> ProductImages, ICollection<ProductIngredientInputDto> ProductIngredients, bool IsLanche);

