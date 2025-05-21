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

        public Pedido(List<LancheCardapio>? lanches, List<ItemComplementoCardapio>? complementos)
        {
            Id = Guid.NewGuid();

            if(lanches is not null)
            {
                foreach (LancheCardapio lanche in lanches)
                {
                    this.AdicionarLanche(lanche);
                }
            }
            if (complementos is not null)
            {
                foreach (ItemComplementoCardapio complemento in complementos)
                {
                    this.AdicionarComplemento(complemento);
                }
            }
    
            StatusPedido = Enums.EStatusPedido.EmAberto;
        }

        public Guid Id { get; }

        private readonly List<LanchePedido> _lanches = new();
        public IReadOnlyCollection<LanchePedido> Lanches => _lanches;

        private readonly List<ItemComplementoPedido> _acompanhamentos = new();
        public IReadOnlyCollection<ItemComplementoPedido> Acompanhamentos => _acompanhamentos;

        public Enums.EStatusPedido StatusPedido { get; set; }

        public void AdicionarLanche(Domain.Entidades.ItemCardapio.LancheCardapio lancheBase)
        {
            var lanche = new LanchePedido(lancheBase, this.Id );
            _lanches.Add(lanche);
        }

        public void AdicionarComplemento(Domain.Entidades.ItemCardapio.ItemComplementoCardapio itemCardapioBase)
        {
            var acompanhamento = new ItemComplementoPedido(itemCardapioBase, this.Id);
            _acompanhamentos.Add(acompanhamento);
        }
    }
}
