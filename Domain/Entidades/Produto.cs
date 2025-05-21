using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class Produto
    {
        public Produto(string nome, decimal preco, Categoria categoria, List<Imagem>? imagens, string descricao, List<Ingrediente>? ingredientes = null)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Imagens = imagens;
            Categoria = categoria;
            Ingredientes = ingredientes;
        }
        public Guid Id { get; }
        public string Nome { get; set; }

        public decimal Preco { get; set; }
        public Categoria Categoria { get; set; }

        public List<Imagem>? Imagens { get; set; }

        public string Descricao { get; set; }

        public List<Ingrediente>? Ingredientes { get; set; }
    }
}
