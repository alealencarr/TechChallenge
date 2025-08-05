using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Product.Request
{
    public record ProductRequestDto
    {
        [Required(ErrorMessage = "É necessário informar o nome.")]
        [MaxLength(255)]

        public required string Name { get; set; }

        [Required(ErrorMessage = "É necessário informar o preço.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "É necessário informar a categoria.")]
        public Guid CategorieId { get; set; }

        [Required(ErrorMessage = "É necessário informar a descrição.")]
        [MaxLength(255)]
        public required string Description { get; set; }

        public List<ProductImageRequestDto>? Images { get; set; }
        public List<ProductIngredientRequestDto>? Ingredients { get; set; }
    }
}

 