namespace Shared.DTO.Payment
{
    public record PaymentOutputDto(Guid Id, Guid OrderId, decimal Amount, MethodPaymentDto PaymentMethod, StatusPaymentDto PaymentStatus);

}
