using Contracts.DTO.Pedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Pedido.Criar
{
    public interface ICriarHandler
    {
        Task<Contracts.Response<CriacaoPedidoDTO?>> Handle(CriarCommand command);
    }
}
