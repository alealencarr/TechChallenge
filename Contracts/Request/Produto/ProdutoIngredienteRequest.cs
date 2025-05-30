using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Request.Produto
{
    public class ProdutoIngredienteRequest
    {
        public Guid IdProduto { get; set; }

        public Guid IdIngrediente { get; set; }

    }
}
