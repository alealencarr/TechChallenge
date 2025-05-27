using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Cliente.Criar
{
    public class CriarCommand
    {
        public string CPF { get; set; }  

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public CriarCommand(string cpf, string nome, string email)
        {
            CPF = cpf;
            Nome = nome;
            Email = email;
        }

    }
}
