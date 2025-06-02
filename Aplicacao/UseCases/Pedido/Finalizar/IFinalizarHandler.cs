using Contracts.DTO.Pedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Pedido.Finalizar
{
    public interface IFinalizarHandler
    {
        Task<Contracts.Response<QRCodeDTO?>> Handle(string id);
    }
}
