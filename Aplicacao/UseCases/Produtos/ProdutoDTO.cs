using Domain.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Produtos
{
    public class ProdutoDTO
    {
        public ProdutoDTO(string nome, decimal preco, Domain.Entidades.Categoria categoria, List<Imagem>? imagens, string descricao, List<Ingrediente>? ingredientes = null, string id)
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
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Ingrediente>? Ingredientes { get; set; }
    }
}
