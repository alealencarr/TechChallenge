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
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _appDbContext;

        public CategoriaRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Categoria>> GetAll()
        {
            return await _appDbContext.Categorias.AsNoTracking().ToListAsync();            
        }

        public async Task<Categoria?> GetById(string id)
        {
             var query = _appDbContext.Categorias.AsNoTracking().Where(x => x.Id.ToString() ==  id);

             return await query.FirstOrDefaultAsync();
        }
    }
}
