namespace Contracts.DTO.Pedido
{
    public class CriacaoPedidoDTO
    {
        public Guid Id { get; set; }

        public decimal ValorPedido { get; set; }

        public string StatusPedido { get; set; }
    }
}
