using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Cliente.AlterarCliente
{
    public class AlterarClienteCommand
    {
        public string CPF { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public AlterarClienteCommand(string cpf, string nome, string email)
        {
            CPF = cpf;
            Nome = nome;
            Email = email;
        }
    }
}
