using System.ComponentModel.DataAnnotations;

namespace Contracts.Request.Ingrediente
{
    public class AlterarRequest
    {
        [Required(ErrorMessage = "É necessário informar o novo preço.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "É necessário informar o novo nome.")]
        [MaxLength(255)]

        public required string Nome { get; set; }
    }
}
