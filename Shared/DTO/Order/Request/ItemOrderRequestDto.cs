using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Order.Request
{
    public record ItemOrderRequestDto
    {
        [Required(ErrorMessage = "Favor informar o Id dos itens que compõem este pedido.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Favor informar a Quantidade dos itens que compõem este pedido.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior ou igual a 1.")]
        public int Quantity { get; set; }

        public List<IngredientSnackRequestDto> IngredientsSnack { get; set; } = new();
    }
}
