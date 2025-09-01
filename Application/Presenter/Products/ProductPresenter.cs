using Application.Common;
using Domain.Entities.Aggregates.AggregateProduct;
using Shared.DTO.Categorie.Output;
using Shared.DTO.Product.Output;
using Shared.Result;

namespace Application.Presenter.Products
{
    public class ProductPresenter
    {
        private string _message;
        public ProductPresenter(string? message = null) { _message = message ?? string.Empty; }

        public ICommandResult RetornoSucess()
        {
            return CommandResult.Success(_message);
        }

        public ICommandResult<ProductOutputDto> TransformObject(Product product)
        {
            return CommandResult<ProductOutputDto>.Success(Transform(product), _message);
        }

        public ICommandResult<List<ProductOutputDto>> TransformList(List<Product> products)
        {
            return CommandResult<List<ProductOutputDto>>.Success(products.Select(x => Transform(x)).ToList());
        }

        public ProductOutputDto Transform(Product product)
        {
            return new ProductOutputDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Categorie = new CategorieOutputDto(product.Categorie.Id, product.Categorie.Name, product.Categorie.IsEditavel),
                Ingredients = product.ProductIngredients.Select(x => new ProductIngredientOutputDto(x.IngredientId, x.Quantity)).ToList(),
                Images = product.ProductImages.Select(x => new ProductImageOutputDto($"/{x.ImagePath}/{x.FileName}".ToAbsoluteUrl(), x.Name)).ToList()
            };
        }

        public ICommandResult<T> Error<T>(string message)
        {
            return CommandResult<T>.Fail(message);
        }

        public ICommandResult Error(string message)
        {
            return CommandResult.Fail(message);
        }
    }
}
