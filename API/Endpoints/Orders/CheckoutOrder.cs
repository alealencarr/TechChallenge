using Application.Controllers.Orders;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Infrastructure.Configurations;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Order.Output.CheckoutOrder;
using Shared.Result;

namespace API.Endpoints.Orders;
internal sealed class CheckoutOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/orders/checkout/{id}",
           async (AppDbContext appDbContext, FileStorageSettings _settings, [FromRoute] Guid id) =>
           {
               var httpClient = new HttpClient { BaseAddress = new Uri(QrCodeSettings.Url) };

               IFileStorageService _fileStorage = new FileStorageService(_settings);
               IOrderDataSource dataSource = new OrderDataSource(appDbContext);              
               IPaymentDataSource dataSourcePayment = new PaymentDataSource(httpClient);

               OrderController _orderController = new OrderController(dataSource, dataSourcePayment, _fileStorage);

               var order = await _orderController.CheckoutOrder(id);

               return order.Succeeded ? Results.Ok(order) : Results.BadRequest(order);

           })
           .WithTags("Orders")
           .Produces<ICommandResult<QrCodeOrderOutputDto?>>()
           .WithName("Order.Checkout");
    }
}

 