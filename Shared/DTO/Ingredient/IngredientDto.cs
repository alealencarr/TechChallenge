using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Ingredient;
[ExcludeFromCodeCoverage]
public record IngredientDto(Guid Id, DateTime CreatedAt, string Name, decimal Price);

 