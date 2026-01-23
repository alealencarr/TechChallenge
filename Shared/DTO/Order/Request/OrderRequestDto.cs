using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Order.Request
{
    [ExcludeFromCodeCoverage]
    public record OrderRequestDto
    {
        [Required(ErrorMessage = "Favor informar ao menos um Item para o pedido")]
        public List<ItemOrderRequestDto> Itens { get; set; } = null!;

        public Guid? CustomerId { get; set; } // Opcional, cliente pode não ser cadastrado
    }
}

 