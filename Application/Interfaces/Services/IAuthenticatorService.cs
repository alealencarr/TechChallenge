using Shared.DTO.Authentication.Output;
using Shared.Result;

namespace Application.Interfaces.Services
{
    public interface IAuthenticatorService
    {
        Task<CommandResult<TokenDto?>> RefreshToken(string refreshToken);
        Task<CommandResult<TokenDto?>> Token(string clientId, string clientSecret);
        Task<CommandResult<TokenDto?>> TokenCustomer(string? cpf);
    }
}
