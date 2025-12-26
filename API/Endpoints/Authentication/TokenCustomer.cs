using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Authentication.Output;
using Shared.Result;

namespace API.Endpoints.Authentication;

internal sealed class TokenCustomer : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/authentication/tokencustomer",
           async (IAuthenticatorService _authService, [FromBody] string cpf) =>
           {
               var token = await _authService.TokenCustomer(cpf);

               return token.Succeeded ? Results.Ok(token) : Results.BadRequest(token);

           })
           .WithTags("Authentication")
           .Produces<ICommandResult<TokenDto?>>()
           .WithName("Authentication.TokenCustomer");
    }
}
 