using Aplicacao.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Cliente.Alterar
{
    public class AlterarCommand
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

 
        public AlterarCommand(string cpf, string nome, string email)
        {
            Nome = nome;
            Email = email;
        }

    }
}
