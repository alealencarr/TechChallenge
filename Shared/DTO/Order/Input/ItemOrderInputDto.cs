 
namespace Shared.DTO.Order.Input;

public record ItemOrderInputDto(Guid Id, Guid OrderId, Guid ProductId, int Quantity, decimal Price, ICollection<IngredientSnackInputDto>? IngredientsSnack); 


 
  
