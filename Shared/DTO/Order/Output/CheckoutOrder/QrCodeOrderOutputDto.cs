using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Order.Output.CheckoutOrder
{
    [ExcludeFromCodeCoverage]
    public record QrCodeOrderOutputDto(Guid Id, string Url);
}
