using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Product.Request
{
    [ExcludeFromCodeCoverage]
    public record ProductImageRequestDto
    {
        [Required(ErrorMessage = "Favor informar o nome da imagem.")]
        public required string Name { get; set; }
        public byte[] Blob { get; set; } = default!;
    }
}

 