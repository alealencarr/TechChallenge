using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.Agregados.AgregadoProduto
{
    public class ProdutoIngrediente
    {
        public int Quantidade { get; set; }
        public Guid ProdutoId { get; set; }
        public Produto? Produto { get; set; }
        public Guid IngredienteId { get; set; }
        public Ingrediente? Ingrediente { get; set; }
        public ProdutoIngrediente(Guid produtoId, Guid ingredienteId, int quantidade)
        {
            ProdutoId = produtoId;
            IngredienteId = ingredienteId;
            Quantidade = quantidade;
        }

        protected ProdutoIngrediente() { }
    }
}
