using Domain.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Produtos.Alterar
{
    public class AlterarPorIdCommand
    {
        public AlterarPorIdCommand(string nome, decimal preco, Domain.Entidades.Categoria categoria, List<Imagem>? imagens, string descricao, string id, List<Ingrediente>? ingredientes = null)
        {
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Imagens = imagens;
            Categoria = categoria;
            Ingredientes = ingredientes;
            Id = id;
        }
        public string Id { get; set; }
         public string Nome { get; set; }

         public decimal Preco { get; set; }

         public Domain.Entidades.Categoria Categoria { get; set; }

        public List<Imagem>? Imagens { get; set; }

         public string Descricao { get; set; }

        // Só necessário se for "Lanche"
        public List<Ingrediente>? Ingredientes { get; set; }
    }
}
