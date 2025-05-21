using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Cliente.CriarCliente
{
    public class ClienteDTO
    {
        public string CPF { get; set; } = string.Empty;

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Guid Id { get; set; } 
        public ClienteDTO(string cpf, string nome, string email, Guid id)
        {
            CPF = cpf;
            Nome = nome;
            Email = email;
            Id = id;
        }
    }
}
