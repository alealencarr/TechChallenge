using Application.Controllers.Orders;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Order.Output.OrderCompleted;
using Shared.Result;

namespace API.Endpoints.Orders;
internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/orders/{id}",
           async (AppDbContext appDbContext, [FromRoute] Guid id) =>
           {
               IOrderDataSource dataSource = new OrderDataSource(appDbContext);
               OrderController _orderController = new OrderController(dataSource);
               var order = await _orderController.GetOrderById(id);

               return order.Succeeded ? Results.Ok(order) : Results.NotFound(order);

           })
           .WithTags("Orders")
           .Produces<ICommandResult<OrderOutputDto?>>()
           .WithName("Order.GetById")
           .RequireAuthorization();
    }
}
