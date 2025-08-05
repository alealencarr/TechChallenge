using Application.Interfaces.DataSources;
using Domain.Entities.Aggregates.AggregateProduct;
using Infrastructure.DbContexts;
using Infrastructure.DbModels;
using Infrastructure.DbModels.ProductModelsAggregate;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Product.Input;

namespace Infrastructure.DataSources
{
    public class ProductDataSource : IProductDataSource
    {
        private readonly AppDbContext _appDbContext;

        public ProductDataSource(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Update(ProductInputDto product)
        {
            var productDb = await _appDbContext.Product
            .Include(p => p.ProductIngredients)
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.Id == product.Id)
            ?? throw new Exception("Product not found.");

            // Atualiza campos simples
            productDb.Name = product.Name;
            productDb.Price = product.Price;
            productDb.CategorieId = product.CategorieId;
            productDb.Description = product.Description;

            if (productDb.ProductIngredients.Count > 0)
                _appDbContext.ProductIngredients.RemoveRange(productDb.ProductIngredients);

            if (productDb.ProductImages.Count > 0)
                _appDbContext.ProductImages.RemoveRange(productDb.ProductImages);


            _appDbContext.ProductIngredients.AddRange(product.ProductIngredients.GroupBy(x => x.IngredientId).Select(group => new ProductIngredientDbModel(product.Id, group.Key, group.Sum(x => x.Quantity))).ToList());
            _appDbContext.ProductImages.AddRange(product.ProductImages.Select(x => new ProductImageDbModel(x.Id, x.ProductId, x.Blob, x.Name, x.ImagePath, x.MimeType, x.FileName)).ToList());

            //productDb.ProductImages = product.ProductImages.Select(x => new ProductImageDbModel(x.Id, x.ProductId, x.Blob, x.Name, x.ImagePath, x.MimeType, x.FileName)).ToList();
            //productDb.ProductIngredients = product.ProductIngredients.GroupBy(x => x.IngredientId).Select(group => new ProductIngredientDbModel(product.Id, group.Key, group.Sum(x => x.Quantity))).ToList();


            await _appDbContext.SaveChangesAsync();
        }
        public async Task Create(ProductInputDto product)
        {
            var productDbModel = new ProductDbModel(
               product.Id,
               product.Name,
               product.Price,
               product.CategorieId,
               product.Description,
               product.CreatedAt,
               // Imagens (sem necessidade de agrupamento)
               product.ProductImages.Select(x => new ProductImageDbModel(
                   x.Id, x.ProductId, x.Blob, x.Name, x.ImagePath, x.MimeType, x.FileName)).ToList(),
               // Ingredientes (com agrupamento)
               product.ProductIngredients
                   .GroupBy(x => x.IngredientId)
                   .Select(group => new ProductIngredientDbModel(
                       product.Id,
                       group.Key,
                       group.Sum(x => x.Quantity)
                   )).ToList()
           );

            await _appDbContext.AddAsync(productDbModel);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var productDb = await _appDbContext.Product.Where(x => x.Id == id).FirstOrDefaultAsync() ?? throw new Exception("Product not find by Id.");

            _appDbContext.Remove(productDb);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<ProductInputDto?> GetById(Guid id)
        {

            var product = await _appDbContext.Product.AsNoTracking().Include(p => p.Categorie)
                .Include(p => p.ProductIngredients)
                    .ThenInclude(pi => pi.Ingredient)
                .Include(p => p.ProductImages).Where(x => x.Id == id).FirstOrDefaultAsync();

            return product is not null ? new ProductInputDto(product.Id, product.CreatedAt, product.Name, product.Description, product.Price, product.CategorieId,
                product.ProductImages.Select(x => new ProductImageInputDto(x.Id, x.FileName, x.MimeType, x.ImagePath, x.Name, x.Blob, x.ProductId)).ToList(),
                product.ProductIngredients.Select(x => new ProductIngredientInputDto(x.IngredientId, x.Quantity, x.ProductId)).ToList(), new Shared.DTO.Categorie.Input.CategorieInputDto(product.Categorie.Id, product.Categorie.Name, product.Categorie.CreatedAt)) : null;
        }

        public async Task<List<ProductInputDto>> GetByCategorie(string? id, string? name)
        {
            var query = _appDbContext.Product
                .AsNoTracking()
                .Include(p => p.Categorie)
                .Include(p => p.ProductIngredients)
                    .ThenInclude(pi => pi.Ingredient)
                .Include(p => p.ProductImages)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(x => x.CategorieId.ToString() == id);
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Categorie!.Name.ToLower().Contains(name.ToLower()));
            }
            else
                query = query.Where(x => true);


            var products = await query.ToListAsync();

            return products.Select(x => new ProductInputDto(x.Id, x.CreatedAt, x.Name, x.Description, x.Price, x.CategorieId, x.ProductImages.Select(k => new ProductImageInputDto(k.Id, k.FileName, k.MimeType, k.ImagePath, k.Name, k.Blob, k.ProductId)).ToList(),
             x.ProductIngredients.Select(o => new ProductIngredientInputDto(o.IngredientId, o.Quantity, o.ProductId)).ToList(), new Shared.DTO.Categorie.Input.CategorieInputDto(x.Categorie.Id, x.Categorie.Name, x.Categorie.CreatedAt)
             )).ToList();

        }

        public async Task<List<ProductInputDto>> GetByIds(List<Guid> ids)
        {
            var products = await _appDbContext.Set<ProductDbModel>()
                .AsNoTracking()
                .Include(p => p.Categorie)
                .Include(p => p.ProductIngredients)
                    .ThenInclude(pi => pi.Ingredient)
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            return products.Select(x => new ProductInputDto(x.Id, x.CreatedAt, x.Name, x.Description, x.Price, x.CategorieId, x.ProductImages.Select(k => new ProductImageInputDto(k.Id, k.FileName, k.MimeType, k.ImagePath, k.Name, k.Blob, k.ProductId)).ToList(),
                x.ProductIngredients.Select(o => new ProductIngredientInputDto(o.IngredientId, o.Quantity, o.ProductId)).ToList(), new Shared.DTO.Categorie.Input.CategorieInputDto(x.Categorie.Id,x.Categorie.Name,x.Categorie.CreatedAt)
                )).ToList();
        }
 
    }
}
