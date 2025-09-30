using Application.Interfaces.DataSources;
using Domain.Entities.Aggregates.AggregateProduct;
using Shared.DTO.Product.Input;
using System.Linq;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace Application.Gateways
{
    public class ProductGateway
    {
        private IProductDataSource _dataSource;

        private ProductGateway(IProductDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        public static ProductGateway Create(IProductDataSource dataSource)
        {
            return new ProductGateway(dataSource);
        }

        public async Task<List<Product>> GetByIds(List<Guid> ids)
        {
            var products = await _dataSource.GetByIds(ids);

            return products.Select(x => new Product(x.Id, x.Name, x.Description, x.Price, x.CategorieId, x.CreatedAt,
             x.ProductIngredients.Select(k => new ProductIngredient(k.ProductId, k.IngredientId, k.Quantity)).ToList(),
             x.ProductImages.Select(l => new ProductImage(l.Id, l.ProductId, l.Blob, l.Name, l.ImagePath, l.MimeType, l.FileName)).ToList(), new Domain.Entities.Categorie(x.Categorie.Name, x.Categorie.Id, x.Categorie.CreatedAt, x.Categorie.IsEditavel)
             )).ToList();
        }

        public async Task<Product?> GetById(Guid id)
        {
            var product = await _dataSource.GetById(id);

            return product is not null ? new Product(product.Id, product.Name, product.Description, product.Price, product.CategorieId, product.CreatedAt,
                product.ProductIngredients.Select(x => new ProductIngredient(x.ProductId, x.IngredientId, x.Quantity)).ToList(),
                product.ProductImages.Select(x => new ProductImage(x.Id, x.ProductId, x.Blob, x.Name, x.ImagePath, x.MimeType, x.FileName)).ToList(), new Domain.Entities.Categorie(product.Categorie.Name, product.Categorie.Id, product.Categorie.CreatedAt, product.Categorie.IsEditavel)) : null;
        }

        public async Task Delete(Guid id)
        {
            await _dataSource.Delete(id);
        }

        public async Task<List<Product>> GetByCategorie(string? id, string? name)
        {
            var products = await _dataSource.GetByCategorie(id, name);

            return products.Select(x => new Product(x.Id, x.Name, x.Description, x.Price, x.CategorieId, x.CreatedAt,
               x.ProductIngredients.Select(k => new ProductIngredient(k.ProductId, k.IngredientId, k.Quantity)).ToList(),
               x.ProductImages.Select(l => new ProductImage(l.Id, l.ProductId, l.Blob, l.Name, l.ImagePath, l.MimeType, l.FileName)).ToList(), new Domain.Entities.Categorie(x.Categorie.Name, x.Categorie.Id, x.Categorie.CreatedAt, x.Categorie.IsEditavel)
               )).ToList();
        }
 
        public async Task CreateProduct(Product product)
        {
            var productInput = new ProductInputDto(product.Id, product.CreatedAt, product.Name, product.Description, product.Price, product.CategorieId, 
                product.ProductImages.Select(x => new ProductImageInputDto(x.Id, x.FileName, x.MimeType, x.ImagePath, x.Name, x.Blob, x.ProductId )).ToList() ,
                product.ProductIngredients.Select(x => new ProductIngredientInputDto(x.IngredientId, x.Quantity, x.ProductId)).ToList(), new Shared.DTO.Categorie.Input.CategorieInputDto(product.Categorie.Id, product.Categorie.Name, product.Categorie.IsEditavel, product.Categorie.CreatedAt)
              );            

            await _dataSource.Create(productInput);
        }

        public async Task UpdateProduct(Product product)
        {
            var productInput = new ProductInputDto(product.Id, product.CreatedAt, product.Name, product.Description, product.Price, product.CategorieId,
                product.ProductImages.Select(x => new ProductImageInputDto(x.Id, x.FileName, x.MimeType, x.ImagePath, x.Name, x.Blob, x.ProductId)).ToList(),
                product.ProductIngredients.Select(x => new ProductIngredientInputDto(x.IngredientId, x.Quantity, x.ProductId)).ToList(), new Shared.DTO.Categorie.Input.CategorieInputDto(product.Categorie.Id, product.Categorie.Name, product.Categorie.IsEditavel, product.Categorie.CreatedAt)
              );

            await _dataSource.Update(productInput);
        }

    }
}
