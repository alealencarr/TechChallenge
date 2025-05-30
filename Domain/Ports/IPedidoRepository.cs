using Domain.Entidades.Agregados.AgregadoPedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports
{
    public interface IPedidoRepository
    {
        Task Adicionar(Pedido pedido);
        Task AlterarStatus(Pedido pedido);
        Task<Pedido?> GetById (string id);
    }
}
