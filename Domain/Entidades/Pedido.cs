using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class Pedido
    {

        public Pedido(Domain.Entidades.Cliente? cliente)
        { 
            Cliente = cliente;
            StatusPedido = Enums.EStatusPedido.EmAberto;
            Id = Guid.NewGuid();
        }

        private readonly List<ItemPedido> _itens = new();
        public IReadOnlyCollection<ItemPedido> Itens => _itens;

        public Guid Id { get; private set; }
        public Enums.EStatusPedido StatusPedido { get; set; }

        public Cliente? Cliente { get; set; }

        public decimal PrecoPedido { get; private set; } = 0M;
        public void AdicionarItem(Domain.Entidades.ItemPedido itemBase)
        {
            PrecoPedido += itemBase.ObterPrecoTotal();

            _itens.Add(itemBase);
        }
        
        
    }
}
