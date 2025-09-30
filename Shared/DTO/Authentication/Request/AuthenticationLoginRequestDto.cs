using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Authentication.Request
{
    public record AuthenticationLoginRequestDto
    {
        [Required(ErrorMessage = "Favor informar o e-mail do usuário.")]
        public required string ClientId { get; set; }

        [Required(ErrorMessage = "Favor informar a senha do usuário.")]
        [MaxLength(30, ErrorMessage = "A senha deve ter no máximo 30 caracteres")]
        [MinLength(8, ErrorMessage = "A senha deve ter no minimo 8 caracteres")]
        public required string ClientSecret { get; set; }
    }
}
