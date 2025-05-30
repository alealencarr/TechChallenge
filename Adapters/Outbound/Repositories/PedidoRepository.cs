using Domain.Entidades.Agregados.AgregadoPedido;
using Domain.Ports;
using Infraestrutura;
using Microsoft.EntityFrameworkCore;
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

      

        public async Task AlterarStatus(Pedido pedido)
        {
            _appDbContext.Pedidos.Update(pedido);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Pedido?> GetById(string id)
        {
            return await _appDbContext.Pedidos
                       .Include(p => p.Cliente)
                .Include(p => p.Itens)
                    .ThenInclude(pi => pi.Ingredientes) 
                .FirstOrDefaultAsync(x => x.Id.ToString() == id);
        }
    }
}
