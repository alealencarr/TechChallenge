using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Payment
{
    [ExcludeFromCodeCoverage]
    public record PaymentOutputDto(Guid Id, Guid OrderId, decimal Amount, MethodPaymentDto PaymentMethod, StatusPaymentDto PaymentStatus);

}
