using Shared.DTO.Order.Request;

namespace Application.UseCases.Orders.Command;

public class OrderCommand
{
    public OrderCommand(Guid? customerId, List<ItemOrderRequestDto> items)
    {
        Itens = items;
        CustomerId = customerId;
    }

    public List<ItemOrderRequestDto> Itens { get; set; } = null!;

    public Guid? CustomerId { get; set; }
}



