using Domain.Entidades.Agregados.AgregadoProduto;
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
            var produtoExistente = await _appDbContext.Produtos
                 .Include(p => p.ProdutoIngredientes)
                 .Include(p => p.ProdutoImagens)
                 .FirstOrDefaultAsync(p => p.Id == produto.Id);

 
            if (produtoExistente.ProdutoIngredientes.Count > 0)
                _appDbContext.ProdutoIngredientes.RemoveRange(produtoExistente.ProdutoIngredientes);

            if (produtoExistente.ProdutoImagens.Count > 0)
                _appDbContext.ProdutoImagens.RemoveRange(produtoExistente.ProdutoImagens);

            _appDbContext.Entry(produtoExistente).CurrentValues.SetValues(produto);

            produtoExistente.VinculaIngredientes(produto.ProdutoIngredientes);
            produtoExistente.VinculaImagens(produto.ProdutoImagens);

            await _appDbContext.SaveChangesAsync();

        }

        public async Task<List<Produto>> Buscar(string? id, string? name)
        {
            var query = _appDbContext.Produtos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Include(p => p.ProdutoIngredientes)
                    .ThenInclude(pi => pi.Ingrediente)
                .Include(p => p.ProdutoImagens)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(x => x.CategoriaId.ToString() == id);
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Categoria.Nome.ToUpper().Contains(name.ToUpper()));
            }
            else
                query = query.Where(x => true);


            return await query.ToListAsync();
        }
        public async Task<Produto?> GetById(string id)
        {
            return await _appDbContext.Produtos
            .AsNoTracking()
            .Include(p => p.Categoria)
                .Include(p => p.ProdutoIngredientes)
                    .ThenInclude(pi => pi.Ingrediente)
                .Include(p => p.ProdutoImagens)
            .FirstOrDefaultAsync(x => x.Id.ToString() == id);             
        }
        

        public async Task Remover(Produto produto)
        {
            _appDbContext.Produtos.Remove(produto);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Produto>> GetByIds(List<Guid> ids)
        {
            return await _appDbContext.Set<Produto>()
                .AsNoTracking()
            .Include(p => p.Categoria)
                .Include(p => p.ProdutoIngredientes)
                    .ThenInclude(pi => pi.Ingrediente)
                .Include(p => p.ProdutoImagens)
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();
        }
    }
}
