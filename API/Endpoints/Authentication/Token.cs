using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Shared.DTO.Authentication.Output;
using Shared.DTO.Authentication.Request;
using Shared.Result;

namespace API.Endpoints.Authentication;
internal sealed class Token : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/authentication/token",
           async (IAuthenticatorService _authService, [FromBody] AuthenticationLoginRequestDto authenticationDto) =>
           {
               //if (!MiniValidator.TryValidate(authenticationDto, out var errors))
               //    return Results.ValidationProblem(errors);
               // Responsabilidade da Application 

               var token = await _authService.Token(authenticationDto.ClientId, authenticationDto.ClientSecret);

               return token.Succeeded ? Results.Ok(token) : Results.BadRequest(token);

           })
           .WithTags("Authentication")
           .Produces<ICommandResult<TokenDto?>>()
           .WithName("Authentication.Token");
    }
}

