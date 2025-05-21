 
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
    public class AlterarPorCPFCommand
    {
        public string Nome { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Cpf { get; private set; } = string.Empty;

        public AlterarPorCPFCommand(string cpf, string nome, string email)
        {
            Cpf = cpf;
            Nome = nome;
            Email = email;
        }

    }
}
