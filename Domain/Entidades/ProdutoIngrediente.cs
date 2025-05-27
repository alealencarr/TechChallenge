using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class ProdutoIngrediente
    {
        public Guid ProdutoId { get; set; }
        public Produto? Produto { get; set; }
        public Guid IngredienteId { get; set; }
        public Ingrediente? Ingrediente { get; set; }
        public ProdutoIngrediente(Guid produtoId, Guid ingredienteId)
        {
            ProdutoId = produtoId;
            IngredienteId = ingredienteId;
        }
    }
}
