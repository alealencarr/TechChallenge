using Domain.Entidades;
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
    public class IngredienteRepository : IIngredienteRepository
    {
        private readonly AppDbContext _appDbContext;

        public IngredienteRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Ingrediente?> GetById(string Id)
        {
            return await _appDbContext.Ingredientes.AsNoTracking().FirstOrDefaultAsync(x => x.Id.ToString() == Id);
        }

        public async Task<List<Ingrediente>?> GetAll()
        {
            return await _appDbContext.Ingredientes.AsNoTracking().ToListAsync();
        }
        public async Task Adicionar(Ingrediente ingrediente)
        {
            await _appDbContext.Ingredientes.AddAsync(ingrediente);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Alterar(Ingrediente ingrediente)
        {
            _appDbContext.Ingredientes.Update(ingrediente);
            await _appDbContext.SaveChangesAsync();
        }

    }
}
