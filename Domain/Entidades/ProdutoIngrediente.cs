using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class ProdutoIngrediente
    {
        public Guid IdProduto { get; set; }
        public Produto Produto { get; set; }
        public Guid IdIngrediente { get; set; }
        public Ingrediente Ingrediente { get; set; }
        public ProdutoIngrediente(Guid idProduto, Guid idIngrediente)
        {
            IdProduto = idProduto;
            IdIngrediente = idIngrediente;
        }
    }
}
