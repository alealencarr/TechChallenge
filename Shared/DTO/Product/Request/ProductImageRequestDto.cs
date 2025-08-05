using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Product.Request
{
    public record ProductImageRequestDto
    {
        [Required(ErrorMessage = "Favor informar o nome da imagem.")]
        public required string Name { get; set; }
        public byte[] Blob { get; set; } = default!;
    }
}

 