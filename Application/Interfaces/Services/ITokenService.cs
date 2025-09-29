using Domain.Entities;
using Domain.Entities.Aggregates.AggregateUser;

namespace Application.Interfaces.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// Gera um token para um usuário administrativo (com papéis e permissões).
        /// </summary>
        /// 
        (string AccessToken, string RefreshToken) GenerateAdminTokens(User user);
 
        /// <summary>
        /// Gera um token para um cliente identificado (via CPF).
        /// </summary>
        string GenerateCustomerToken(Customer customer);

        /// <summary>
        /// Gera um token para um cliente anônimo (convidado).
        /// </summary>
        string GenerateGuestToken();
    }
}
