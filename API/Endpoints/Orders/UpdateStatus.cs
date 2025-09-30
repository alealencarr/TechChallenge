using Application.Controllers.Orders;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Order.Output.OrderSummary;
using Shared.Result;

namespace API.Endpoints.Orders;
internal sealed class UpdateStatus : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/orders/updatestatus/{id}",
          async (AppDbContext appDbContext, [FromRoute] Guid id) =>
          {
              IOrderDataSource dataSource = new OrderDataSource(appDbContext);
              OrderController _orderController = new OrderController(dataSource);

              var order = await _orderController.UpdateStatusOrder(id);

              return order.Succeeded ? Results.Ok(order) : Results.BadRequest(order);

          })
          .WithTags("Orders")
          .Produces<ICommandResult<OrderSummaryOutputDto?>>()
          .WithName("Order.UpdateStatus")
          .RequireAuthorization();
    }
}
