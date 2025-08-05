using Shared.DTO.Categorie.Input;

namespace Shared.DTO.Product.Input;

public record ProductInputDto(Guid Id, DateTime CreatedAt, string Name, string Description, decimal Price, Guid CategorieId, ICollection<ProductImageInputDto> ProductImages, ICollection<ProductIngredientInputDto> ProductIngredients, CategorieInputDto Categorie);

