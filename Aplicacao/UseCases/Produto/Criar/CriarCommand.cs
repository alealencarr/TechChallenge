
using Contracts.DTO.Produto;
using Domain.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Aplicacao.UseCases.Produto.Criar
{
    public class CriarCommand
    {
        public CriarCommand(string nome, decimal preco, Guid categoriaId, List<ProdutoImagemDTO>? imagens, string descricao, List<IngredienteCommand>? ingredientes = null)
        {
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Imagens = imagens;
            CategoriaId = categoriaId;
            Ingredientes = ingredientes;
        }
        public string Nome { get; set; }

        public decimal Preco { get; set; }

        public Guid CategoriaId { get; set; }

 
        public string Descricao { get; set; }

        public List<IngredienteCommand>? Ingredientes { get; set; }

        public List<ProdutoImagemDTO>? Imagens { get; set; }

    }
}
