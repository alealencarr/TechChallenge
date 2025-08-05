using Application.Controllers.Orders;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.DTO.Order.Output.OrderSummary;
using Shared.DTO.Order.Request;
using Shared.DTO.Payment;
using Shared.Helpers;
using Shared.Result;
using System.Text;

namespace API.Endpoints.Payment
{
    internal sealed class PostNotification : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/payments/notification",
              async (HttpContext context, AppDbContext appDbContext) =>
              {
                   context.Request.EnableBuffering();  
                   using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
                   var body = await reader.ReadToEndAsync();
                   context.Request.Body.Position = 0;

                   var receivedSignature = context.Request.Headers["X-Signature"].FirstOrDefault();
                   var secretKey = WebhookSettings.SecretKey; 

                   if (string.IsNullOrWhiteSpace(receivedSignature))
                       return Results.Unauthorized();

                   var computedSignature = SignatureGenerator.GenerateSignature(body, secretKey);

                   if (!string.Equals(receivedSignature, computedSignature, StringComparison.OrdinalIgnoreCase))
                       return Results.Unauthorized();

                   var paymentDto = JsonConvert.DeserializeObject<PaymentNotificationDto>(body);
                   
                   IOrderDataSource dataSource = new OrderDataSource(appDbContext);
                   OrderController _orderController = new OrderController(dataSource);
                   var order = await _orderController.UpdatePaymentOrder(paymentDto);

                   return Results.Ok();

               })
               .WithTags("Payment")
               .Produces<ICommandResult>()
               .WithName("Payment.Notification");
        }
    }

}
