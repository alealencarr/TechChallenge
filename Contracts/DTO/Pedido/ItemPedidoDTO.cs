using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO.Pedido
{
    public class ItemPedidoDTO
    {
        public string ProdutoId { get; set; }

        public int Quantidade { get; set; }

        public decimal Preco { get; set;  }

        public IReadOnlyCollection<IngredienteItemPedidoDTO> Ingredientes { get; set; } = [];
        public ItemPedidoDTO(string produtoId, int quantidade, decimal preco, IReadOnlyCollection<IngredienteItemPedidoDTO> ingredientes)
        {
            ProdutoId = produtoId;
            Quantidade = quantidade;
            Preco = preco;
            Ingredientes = ingredientes;
        }

      
    }
}
