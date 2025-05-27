using System.ComponentModel.DataAnnotations;

namespace Contracts.Request.Pedido
{
    public class IngredienteRequest
    {
        [Required(ErrorMessage = "Favor informar o Id do ingrediente.")]
        public Guid Id { get; set; }

        public bool Adicional { get; set; } = false;
    }
}
