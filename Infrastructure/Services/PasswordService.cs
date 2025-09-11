using Application.Interfaces.Services;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        /// <summary>
        /// Gera o hash de uma senha usando BCrypt.
        /// O "salt" é gerado automaticamente e embutido no hash final.
        /// </summary>
        /// <param name="password">A senha em texto plano.</param>
        /// <returns>O hash da senha.</returns>
        public string HashPassword(string password)
        {
            // O segundo parâmetro é o "work factor", que define a complexidade do hash.
            // Um valor entre 10 e 12 é recomendado.
            return BCryptNet.HashPassword(password, 12);
        }

        /// <summary>
        /// Verifica se uma senha em texto plano corresponde a um hash.
        /// </summary>
        /// <param name="password">A senha em texto plano a ser verificada.</param>
        /// <param name="passwordHash">O hash armazenado no banco de dados.</param>
        /// <returns>True se a senha corresponder, false caso contrário.</returns>
        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCryptNet.Verify(password, passwordHash);
        }
    }
}
