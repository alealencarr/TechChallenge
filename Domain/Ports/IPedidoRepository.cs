using Domain.Entidades;
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
         Task Finalizar(Pedido pedido);
    }
}
