using Contracts.DTO.Pedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Pedido.Checkout
{
    public interface ICheckoutHandler
    {
        Task<Contracts.Response<QRCodeDTO?>> Handle(string id);
    }
}
