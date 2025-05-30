using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.Agregados.AgregadoProduto
{
    public class ProdutoImagem
    {
        public Guid IdProduto { get; set; }
        public Produto Produto { get; set; }
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public byte[] Blob { get; set; } = default!;
        public ProdutoImagem(Guid idProduto, byte[] blob, string? nome)
        {
            IdProduto = idProduto;
            Id = Guid.NewGuid();
            Blob = blob;
            Nome = nome;
        }
        protected ProdutoImagem() { }
    }
}
