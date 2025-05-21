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

        public async Task<List<Produto>> Buscar(Categoria? categoria)
        {
            var query = _appDbContext.Produtos
            .AsNoTracking()
            .Where(x => categoria == null || x.Categoria.Id == categoria.Id);

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
