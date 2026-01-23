using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Order.Output
{
    [ExcludeFromCodeCoverage]
    public record OrderStatusOutputDto
    {
        public int Id { get; set; }
        public string Description { get; set; }

    }
}
