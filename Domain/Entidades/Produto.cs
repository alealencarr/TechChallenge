using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class Produto
    {
        public Produto(string nome, decimal preco, Guid categoriaId, Categoria categoria, List<Imagem>? imagens, string descricao )
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Imagens = imagens;
            CategoriaId = categoriaId;
            Categoria = categoria;

        }
        public Guid Id { get; }
        public string Nome { get; set; }

        public decimal Preco { get; set; }
        public Guid CategoriaId { get; set; }
        public Categoria Categoria  { get; set; }

        public ICollection<Imagem>? Imagens { get; set; }

        public string Descricao { get; set; }

        public ICollection<ProdutoIngrediente>? ProdutoIngredientes { get; private set; }

        public void VinculaIngredientes(ICollection<ProdutoIngrediente>? produtoIngredientes)
        {
            ProdutoIngredientes = produtoIngredientes;
        }
    }
}
