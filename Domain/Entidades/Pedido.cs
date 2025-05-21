using Domain.Entidades.ItemCardapio;
using Domain.Entidades.ItemPedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class Pedido
    {
        public Pedido(LanchePedido? lanche, List<ItemComplementoPedido>? complementos)
        { 
            Id = Guid.NewGuid();
            Lanche = lanche;
            Complementos = complementos;
        }

        public Guid Id { get; }
        public LanchePedido? Lanche { get; }
        public List<ItemComplementoPedido>? Complementos { get; set; }

        public Enums.EStatusPedido StatusPedido { get; set; }
    }
}
