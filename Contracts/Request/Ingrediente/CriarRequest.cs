using System.ComponentModel.DataAnnotations;

namespace Contracts.Request.Ingrediente
{
    public class CriarRequest
    {

        [Required(ErrorMessage = "É necessário informar o preço.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "É necessário informar o nome.")]
        public required string Nome { get; set; }
    }
}
