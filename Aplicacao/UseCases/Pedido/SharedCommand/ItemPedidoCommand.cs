using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Pedido.SharedCommand
{
    public  class ItemPedidoCommand
    {
        public Guid ProdutoId { get; set; }
        public List<IngredienteCommand> IngredientesLanche { get; set; } = new();

        public ItemPedidoCommand(Guid produtoId, List<IngredienteCommand> ingredientesLanche)
        {
            ProdutoId = produtoId;
            IngredientesLanche = ingredientesLanche;
        }
    }
}
