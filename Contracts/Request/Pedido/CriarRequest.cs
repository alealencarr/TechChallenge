using Domain.Entidades;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Request.Pedido
{
    public class CriarRequest
    {
        [Required(ErrorMessage = "Favor informar ao menos um Item para o pedido")]
        public List<ItemPedidoRequest> Itens { get; set; } = null!;

        public Guid? ClienteId { get; set; } // Opcional, cliente pode não ser cadastrado
    }

}
