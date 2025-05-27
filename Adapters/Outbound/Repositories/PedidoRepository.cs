using Domain.Entidades;
using Domain.Ports;
using Infraestrutura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapters.Outbound.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _appDbContext;

        public PedidoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Adicionar(Pedido pedido)
        {
            await _appDbContext.Pedidos.AddAsync(pedido);
            await _appDbContext.SaveChangesAsync();
        }

      

        public async Task Finalizar(Pedido pedido)
        {
            throw new NotImplementedException();
        }
    }
}
