
using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Order.Input;
[ExcludeFromCodeCoverage]
public record IngredientSnackInputDto(Guid Id, Guid IngredientId, Guid ItemId, bool Additional, int Quantity, decimal Price);



 
 