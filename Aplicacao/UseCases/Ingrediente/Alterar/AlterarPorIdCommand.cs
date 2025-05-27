 
using Aplicacao.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Ingrediente.Alterar
{
    public class AlterarPorIdCommand
    {
        public string Nome { get; private set; } 
        public decimal Preco { get; private set; }  
        public string Id { get; private set; }  

        public AlterarPorIdCommand(string id, string nome, decimal preco)
        {
            Id = id;
            Nome = nome;
            Preco = preco;
        }

    }
}
