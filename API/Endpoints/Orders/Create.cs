using Application.Controllers.Orders;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Shared.DTO.Order.Output.OrderSummary;
using Shared.DTO.Order.Request;
using Shared.Result;

namespace API.Endpoints.Orders;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/orders",
           async (AppDbContext appDbContext, [FromBody] OrderRequestDto orderDto) =>
           {
               if (!MiniValidator.TryValidate(orderDto, out var errors))
                   return Results.ValidationProblem(errors);
               IOrderDataSource dataSource = new OrderDataSource(appDbContext);
               IProductDataSource dataSourceProduct = new ProductDataSource(appDbContext);
               ICustomerDataSource dataSourceCustomer = new CustomerDataSource(appDbContext);
               IIngredientDataSource dataSourceIngrediente = new IngredientDataSource(appDbContext);

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
