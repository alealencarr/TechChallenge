using Application.Gateways;
using Application.Interfaces.Services;

namespace Application.UseCases.Authentication
{
    public class AuthenticationTokenCustomerUseCase
    {
        CustomerGateway _gateway = null;
        ITokenService _tokenService;
        public static AuthenticationTokenCustomerUseCase Create(CustomerGateway gateway, ITokenService tokenService)
        {
            return new AuthenticationTokenCustomerUseCase(gateway, tokenService);
        }

        private AuthenticationTokenCustomerUseCase(CustomerGateway gateway, ITokenService tokenService)
        {
            _gateway = gateway;
            _tokenService = tokenService;
        }

        public async Task<string> Run(string? cpf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cpf))
                {
                    return _tokenService.GenerateGuestToken();
                }
                else
                {
                    var customer = await _gateway.GetByCpf(cpf);

                    if (customer is null)
                        throw new Exception($"Error: Customer not find by Cpf.");

                    return _tokenService.GenerateCustomerToken(customer);

                }

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
}
