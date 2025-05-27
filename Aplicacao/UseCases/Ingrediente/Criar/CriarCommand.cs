using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Ingrediente.Criar
{
    public class CriarCommand
    {
        public string Nome { get; set; } 

        public decimal Preco { get; set; } 

        public CriarCommand(decimal preco, string nome)
        {
            Nome = nome;
            Preco = preco;
        }

    }
}
