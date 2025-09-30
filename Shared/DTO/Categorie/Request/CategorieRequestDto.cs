using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Categorie.Request
{

    public record CategorieRequestDto
    {
        [Required(ErrorMessage = "Favor informar o Nome da Categoria.")]
        [MinLength(1)]
        public string Name { get; set; } = string.Empty;

        public bool IsEditavel { get; set; }
    }
}
