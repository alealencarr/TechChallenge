namespace Contracts.DTO.Cliente
{
    public class ClienteDTO
    {
        public Guid Id { get; set; }

        public string CPF { get; set; } = string.Empty;

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public ClienteDTO(string cpf, string nome, string email, Guid id )
        {
            CPF = cpf;
            Nome = nome;
            Email = email;
            Id = id;
         }

     }
}
