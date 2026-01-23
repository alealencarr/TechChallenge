using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Payment
{
    [ExcludeFromCodeCoverage]
    public record StatusPaymentDto(int Id, string Description);
}
