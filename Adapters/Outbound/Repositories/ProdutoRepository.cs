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
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProdutoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

        }
        public async Task Adicionar(Produto produto)
        {
            await _appDbContext.Produtos.AddAsync(produto);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Alterar(Produto produto)
        {
            _appDbContext.Produtos.Update(produto);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Produto>> Buscar(string? id, string? name)
        {
            var query = _appDbContext.Produtos.AsNoTracking().AsQueryable();

            if (id != null ^ name != null)
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    query = query.Where(x => x.Categoria.Id.ToString() == id);
                }
                else if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(x => x.Categoria.Nome.Contains(name));
                }
                else
                    query = query.Where(x => true);
            }
            else
                query = query.Where(x => true);

            return await query.ToListAsync();
        }
        public async Task<Produto?> BuscarPorID(string id)
        {
            return await _appDbContext.Produtos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.ToString() == id);             
        }
        

        public async Task Remover(Produto produto)
        {
            _appDbContext.Produtos.Remove(produto);

            await _appDbContext.SaveChangesAsync();
        }
    }
}
