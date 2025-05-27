using Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Pedido.Criar
{
    public class CriarCommand
    {
        public Guid? ClienteId { get; set; }
        public List<Aplicacao.UseCases.Pedido.SharedCommand.ItemPedidoCommand> Itens { get; set; }

        public CriarCommand(Guid? clienteId, List<Aplicacao.UseCases.Pedido.SharedCommand.ItemPedidoCommand> itens)
        {
            ClienteId = clienteId;
            Itens = itens;
        }

    }
}
