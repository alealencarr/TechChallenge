using Application.Gateways;
using Application.Interfaces.Services;

namespace Application.UseCases.Authentication;

public class AuthenticationTokenUseCase
{
    UserGateway _gateway = null;
    IPasswordService _passwordService;
    ITokenService _tokenService;
    public static AuthenticationTokenUseCase Create(UserGateway gateway, IPasswordService passwordService, ITokenService tokenService)
    {
        return new AuthenticationTokenUseCase(gateway, passwordService, tokenService);
    }

    private AuthenticationTokenUseCase(UserGateway gateway, IPasswordService passwordService, ITokenService tokenService)
    {
        _gateway = gateway;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<(string Token, string RefreshToken)> Run(string mail, string password)
    {
        try
        {
            var user = await _gateway.GetUserByMail(mail);

            if (user is null)
                throw new Exception($"Error: User not find by Mail.");

            if (!_passwordService.VerifyPassword(password, user.PasswordHash))
                throw new Exception($"Error: Senha informada está incorreta.");

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
