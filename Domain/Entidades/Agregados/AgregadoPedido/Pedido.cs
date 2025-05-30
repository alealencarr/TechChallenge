using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.Agregados.AgregadoPedido
{
    public class Pedido
    {

        public Pedido(Guid? clienteId)
        { 
            ClienteId = clienteId;

            StatusPedido = Enums.EStatusPedido.EmAberto;
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }

        public DateTime CreatedAt { get; private set; }

        private readonly List<ItemPedido> _itens = new();
        public IReadOnlyCollection<ItemPedido> Itens => _itens;

        public Guid? ClienteId { get; private set; }
        public Guid Id { get; private set; }
        public Enums.EStatusPedido StatusPedido { get; private set; }


        public Cliente? Cliente { get; set; }

        public decimal Valor { get; private set; } = 0M;
        public void AdicionarItem(ItemPedido itemBase)
        {
            Valor += itemBase.ObterPrecoTotal();

            _itens.Add(itemBase);
        }

        public bool AvancarStatus(out string mensagemErro)
        {
            mensagemErro = null;

            switch (StatusPedido)
            {
                case Enums.EStatusPedido.EmAberto:
                    StatusPedido = Enums.EStatusPedido.Recebido;
                    return true;

                case Enums.EStatusPedido.Recebido:
                    StatusPedido = Enums.EStatusPedido.EmPreparacao;
                    return true;

                case Enums.EStatusPedido.EmPreparacao:
                    StatusPedido = Enums.EStatusPedido.Pronto;
                    return true;

                case Enums.EStatusPedido.Pronto:
                    StatusPedido = Enums.EStatusPedido.Finalizado;
                    return true;

                case Enums.EStatusPedido.Finalizado:
                    mensagemErro = "Este pedido já foi finalizado.";
                    return false;

                default:
                    mensagemErro = "Status do pedido inválido.";
                    return false;
            }
        }

    }
}
