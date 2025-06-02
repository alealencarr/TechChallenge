using Contracts.DTO.Pedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Pedido.AlterarStatus
{
    public interface IAlterarStatusHandler
    {
        Task<Contracts.Response<AlteracaoStatusPedidoDTO?>> Handle(string id);
    }
}
