using Contracts.DTO.Pedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Pedido.BuscarPorId
{
    public interface IBuscarPorIdHandler
    {
        Task<Contracts.Response<PedidoDTO?>> Handle(string id);
    }
}
