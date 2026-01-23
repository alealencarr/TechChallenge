using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Payment
{
    [ExcludeFromCodeCoverage]
    public record PaymentNotificationDto(Guid Id, Guid OrderId, int Status, decimal Amount);


}
