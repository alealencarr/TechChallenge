using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO.Produto
{
    public class ProdutoIngredienteDTO
    {
        public Guid IdProduto { get; set; }

        public Guid IdIngrediente { get; set; }

    }
}
