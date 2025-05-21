using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.ItemPedido
{
    public class ItemComplementoPedido
    {
        private ItemComplementoPedido() {} 
        internal ItemComplementoPedido (Domain.Entidades.ItemCardapio.ItemComplementoCardapio itemComplementoBase, Guid id)
        {
            PedidoId = id;
        }

        public Guid PedidoId { get; private set; }

    }
}
