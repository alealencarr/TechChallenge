using Domain.Entidades;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Request.Pedido
{
    public abstract class ItemPedidoRequest
    {
        [Required(ErrorMessage = "Favor informar o Id dos itens que compõem este pedido.")]
        public Guid Id { get; set; }

        public List<IngredienteRequest> Ingredientes { get; set; } = new();
    }
}
