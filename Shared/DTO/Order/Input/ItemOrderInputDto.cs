
using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Order.Input;
[ExcludeFromCodeCoverage]
public record ItemOrderInputDto(Guid Id, Guid OrderId, Guid ProductId, int Quantity, decimal Price, ICollection<IngredientSnackInputDto>? IngredientsSnack); 


 
  
