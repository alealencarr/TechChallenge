 

namespace Contracts.Request.Cliente
{
    public class AlterarRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;


        public AlterarRequest(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }
    }
}
