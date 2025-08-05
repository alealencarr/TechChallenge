namespace Shared.DTO.Payment
{
    public record PaymentNotificationDto(Guid Id, Guid OrderId, int Status, decimal Amount);


}
