using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Product.Input;
[ExcludeFromCodeCoverage]
public record ProductIngredientInputDto(Guid IngredientId, int Quantity, Guid ProductId);


 