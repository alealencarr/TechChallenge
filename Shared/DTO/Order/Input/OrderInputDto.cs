
using Shared.DTO.Payment;
using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Order.Input;
[ExcludeFromCodeCoverage]
public record OrderInputDto(Guid Id, DateTime CreatedAt, int OrderStatus, decimal Price, Guid? CustomerId, ICollection<ItemOrderInputDto> Itens, PaymentInputDto? Payment);
  
 