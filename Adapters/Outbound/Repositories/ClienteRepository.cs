using Domain.Entidades;
using Domain.Ports;
using Infraestrutura;
using Microsoft.EntityFrameworkCore;


namespace Adapters.Outbound.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Adicionar(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task Alterar(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<Cliente?> GetByCPF(string CPF)
        {
           return await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.CPF.Valor == CPF);           
        }

        public async Task<Cliente?> GetById(string id)
        {
            return await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.Id.ToString() == id);
        }
    }
}
