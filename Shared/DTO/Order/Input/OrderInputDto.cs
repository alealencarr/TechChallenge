
using Shared.DTO.Payment;

namespace Shared.DTO.Order.Input;

public record OrderInputDto(Guid Id, DateTime CreatedAt, int OrderStatus, decimal Price, Guid? CustomerId, ICollection<ItemOrderInputDto> Itens, PaymentInputDto? Payment);
  
 