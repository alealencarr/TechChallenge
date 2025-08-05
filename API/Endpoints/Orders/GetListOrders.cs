using Application.Controllers.Orders;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Shared.DTO.Order.Output.OrderSummary;
using Shared.Result;

namespace API.Endpoints.Orders;
internal sealed class GetListOrders : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/orders",
           async (AppDbContext appDbContext) =>
           {
               IOrderDataSource dataSource = new OrderDataSource(appDbContext);
               OrderController _orderController = new OrderController(dataSource);
               var order = await _orderController.GetListOrders();

               return order.Succeeded ? Results.Ok(order) : Results.NotFound(order);

           })
           .WithTags("Orders")
           .Produces<ICommandResult<List<OrderSummaryOutputDto>?>>()
           .WithName("Order.GetListOrders");
    }
}
