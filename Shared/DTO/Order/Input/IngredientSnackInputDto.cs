 
namespace Shared.DTO.Order.Input;

public record IngredientSnackInputDto(Guid Id, Guid IngredientId, Guid ItemId, bool Additional, int Quantity, decimal Price);



 
 