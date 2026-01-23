using Shared.DTO.Categorie.Output;
using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Order.Output.OrderCompleted
{
    [ExcludeFromCodeCoverage]
    public record OrderOutputDto
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal Price { get; set; }

        public Guid? CustomerId { get; set; }
 
        public OrderStatusOutputDto OrderStatus { get; set; }

        public ICollection<ItemOrderOutputDto> Itens { get; set; } = [];

    }
}


 