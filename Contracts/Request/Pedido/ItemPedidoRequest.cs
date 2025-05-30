using Domain.Entidades;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Request.Pedido
{
    public class ItemPedidoRequest
    {
        [Required(ErrorMessage = "Favor informar o Id dos itens que compõem este pedido.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Favor informar a Quantidade dos itens que compõem este pedido.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior ou igual a 1.")]

        public int Quantidade { get; set; }
        public List<IngredienteRequest> Ingredientes { get; set; } = new();
    }
}
