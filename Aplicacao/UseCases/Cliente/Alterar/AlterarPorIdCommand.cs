 
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
    public class AlterarPorIdCommand
    {
        public string Nome { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Id { get; private set; } = string.Empty;

        public AlterarPorIdCommand(string id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }

    }
}
