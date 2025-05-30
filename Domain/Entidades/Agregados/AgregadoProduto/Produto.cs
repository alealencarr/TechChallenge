using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.Agregados.AgregadoProduto
{
    public class Produto
    {
        public Produto(string nome, decimal preco, Guid categoriaId, string descricao )
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            CategoriaId = categoriaId;
            CreatedAt = DateTime.Now;
        }

        protected Produto() { }
        public DateTime CreatedAt { get; private set; }
        public Guid Id { get; private set; }
        public string Nome { get; set; }

        public decimal Preco { get; set; }
        public Guid CategoriaId { get; set; }
        public Categoria? Categoria  { get; set; }

        public string Descricao { get; set; }

        public ICollection<ProdutoImagem> ProdutoImagens { get; private set; } = [];

        public ICollection<ProdutoIngrediente> ProdutoIngredientes { get; private set; } = [];

        public void VinculaIngredientes(ICollection<ProdutoIngrediente> produtoIngredientes)
        {
            ProdutoIngredientes = produtoIngredientes;
        }

        public void VinculaImagens(ICollection<ProdutoImagem> produtoImagens)
        {
            ProdutoImagens = produtoImagens;
        }
    }
}
