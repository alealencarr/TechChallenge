using Domain.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO.Pedido
{
    public class AlteracaoStatusPedidoDTO
    {
        public string Id { get; set; }

        public string StatusPedido { get; set; }

        public AlteracaoStatusPedidoDTO(string id, string statusPedido)
         {
            Id = id;
            StatusPedido = statusPedido;
        }
   }
}
