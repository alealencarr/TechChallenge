namespace Shared.DTO.Payment
{
    public record PaymentInputDto(Guid Id, Guid OrderId, decimal Amount, DateTime CreatedAt, DateTime? PaidAt, int PaymentMethod, int PaymentStatus, byte[] QrBytes, string FileName, string PathRoot);
}
