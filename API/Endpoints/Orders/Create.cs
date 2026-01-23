using Application.Controllers.Orders;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Shared.DTO.Order.Output.OrderSummary;
using Shared.DTO.Order.Request;
using Shared.Result;
using System.Diagnostics.CodeAnalysis;

namespace API.Endpoints.Orders;
[ExcludeFromCodeCoverage]
internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/orders",
           async (AppDbContext appDbContext, IHttpClientFactory _http, [FromBody] OrderRequestDto orderDto) =>
           {
               if (!MiniValidator.TryValidate(orderDto, out var errors))
                   return Results.ValidationProblem(errors);
               IOrderDataSource dataSource = new OrderDataSource(appDbContext);
               IProductDataSource dataSourceProduct = new ProductDataSource(appDbContext);
               ICustomerDataSource dataSourceCustomer = new CustomerDataSource(_http);
               IIngredientDataSource dataSourceIngrediente = new IngredientDataSource(_http);

               OrderController _orderController = new OrderController(dataSource, dataSourceIngrediente, dataSourceCustomer, dataSourceProduct);

               var order = await _orderController.CreateOrder(orderDto);

               return order.Succeeded ? Results.Created($"/{order.Data?.Id}", order) : Results.BadRequest(order);

           })
           .WithTags("Orders")
           .Produces<ICommandResult<OrderSummaryOutputDto?>>()
           .WithName("Order.Create")
           .RequireAuthorization();
    }
}
