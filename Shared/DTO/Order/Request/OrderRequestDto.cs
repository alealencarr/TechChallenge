using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Order.Request
{
    public record OrderRequestDto
    {
        [Required(ErrorMessage = "Favor informar ao menos um Item para o pedido")]
        public List<ItemOrderRequestDto> Itens { get; set; } = null!;

        public Guid? CustomerId { get; set; } // Opcional, cliente pode não ser cadastrado
    }
}

 