using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Customer.Request
{
    public record CustomerRequestDto
    {
        [Required(ErrorMessage = "Favor informar o Cpf.")]
        [MaxLength(14)]
        public required string Cpf { get; set; }

        public string Name { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Favor informar um e-mail válido.")]
        public string Mail { get; set; } = string.Empty;
     }
}
