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
        public AlterarPorIdCommand(string nome, decimal preco, Guid categoriaId, List<Imagem>? imagens, string descricao, string id, List<Guid>? ingredientes = null)
        {
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Imagens = imagens;
            CategoriaId = categoriaId;
            Ingredientes = ingredientes;
        }
        public string Nome { get; set; }
        public string Id { get; set; }

        public decimal Preco { get; set; }

        public Guid CategoriaId { get; set; }

        public List<Imagem>? Imagens { get; set; }

        public string Descricao { get; set; }

        public List<Guid>? Ingredientes { get; set; }
    }
}