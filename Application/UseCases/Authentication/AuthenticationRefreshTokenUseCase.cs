using Application.Gateways;
using Application.Interfaces.Services;

namespace Application.UseCases.Authentication;

public class AuthenticationRefreshTokenUseCase
{
    UserGateway _gateway = null;
    ITokenService _tokenService;
    public static AuthenticationRefreshTokenUseCase Create(UserGateway gateway, ITokenService tokenService)
    {
        return new AuthenticationRefreshTokenUseCase(gateway, tokenService);
    }

    private AuthenticationRefreshTokenUseCase(UserGateway gateway, ITokenService tokenService)
    {
        _gateway = gateway;
        _tokenService = tokenService;
    }

    public async Task<(string Token, string RefreshToken)> Run(string refreshToken)
    {
        try
        {
            var user = await _gateway.GetUserByRefreshToken(refreshToken);

            if (user is null)
                throw new Exception($"Error: User not find by Refresh Token.");

            var tokens = _tokenService.GenerateAdminTokens(user);

            user.SetRefreshToken(tokens.RefreshToken);

            await _gateway.UpdateUser(user);

            return tokens;

        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error:{ex.Message}");
        }
    }
}
