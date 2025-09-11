using Application.Controllers.Orders;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Order.Output.OrderCompleted;
using Shared.Result;

namespace API.Endpoints.Orders
{
    internal sealed class GetStatusPaymentOrder : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/orders/paymentStatus/{id}",
               async (AppDbContext appDbContext, [FromRoute] Guid id) =>
               {
                   IOrderDataSource dataSource = new OrderDataSource(appDbContext);
                   OrderController _orderController = new OrderController(dataSource);
                   var order = await _orderController.GetPaymentStatusOrder(id);

                   return order.Succeeded ? Results.Ok(order) : Results.NotFound(order);

               })
               .WithTags("Orders")
               .Produces<ICommandResult<OrderOutputDto?>>()
               .WithName("Order.GetPaymentStatusOrder")
               .RequireAuthorization();
        }
    }

}
