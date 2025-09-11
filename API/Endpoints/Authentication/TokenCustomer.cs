using API.Endpoints;
using Application.Controllers.Authentication;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Shared.DTO.Authentication.Output;
using Shared.DTO.Authentication.Request;
using Shared.Result;

namespace API.Endpoints.Authentication;
 
internal sealed class TokenCustomer : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/authentication/tokencustomer",
           async (AppDbContext appDbContext, ITokenService tokenService, [FromBody] string cpf) =>
           {
               ICustomerDataSource dataSource = new CustomerDataSource(appDbContext);
               AuthenticationController _userController = new(dataSource, tokenService);
               var user = await _userController.AuthenticationCustomer(cpf);

               return user.Succeeded ? Results.Ok(user) : Results.BadRequest(user);

           })
           .WithTags("Authentication")
           .Produces<ICommandResult<TokenDto?>>()
           .WithName("Authentication.TokenCustomer");                
    }
}
