using Contracts.DTO.Cliente;
using Domain.Entidades.Agregados.AgregadoPedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO.Pedido
{
    public class PedidoDTO
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Valor { get; set; }
        public string StatusPedido { get; set; }
        public ClienteDTO? Cliente { get; set; }
        public IReadOnlyCollection<ItemPedidoDTO> Itens { get; set; } = [];

        public PedidoDTO(Guid id, DateTime createdAt, decimal valor, string statusPedido, ClienteDTO? cliente, IReadOnlyCollection<ItemPedidoDTO> itens)
        {
            this.Id = id;
            this.CreatedAt = createdAt;
            this.Valor = valor;
            this.StatusPedido = statusPedido;
            this.Cliente = cliente;
            this.Itens = itens;
        }
    }
}
