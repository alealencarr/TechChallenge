using Application.Controllers.Products;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Shared.Result;

namespace API.Endpoints.Products;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/products/{id}",
           async (AppDbContext appDbContext, [FromRoute] Guid id) =>
           {
               IProductDataSource dataSource = new ProductDataSource(appDbContext);
               ProductController _productController = new ProductController(dataSource);
               var product = await _productController.DeleteProduct(id);

               return product.Succeeded ? Results.NoContent() : Results.BadRequest(product);

           })
           .WithTags("Products")
           .Produces<ICommandResult>()
           .WithName("Product.Delete");
    }
}
