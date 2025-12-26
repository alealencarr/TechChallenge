using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Authentication.Request
{
    public record RefreshTokenDto
    {
        [Required(ErrorMessage = "Favor informar o Refresh Token no corpo da requisição.")]
        public required string RefreshToken { get; set; }
    }
}
