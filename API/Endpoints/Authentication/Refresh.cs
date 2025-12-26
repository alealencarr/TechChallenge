using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Shared.DTO.Authentication.Output;
using Shared.DTO.Authentication.Request;
using Shared.Result;

namespace API.Endpoints.Authentication;

internal sealed class Refresh : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/authentication/refresh",
           async (IAuthenticatorService _authService, [FromBody] RefreshTokenDto refreshTokenDto) =>
           {
               //if (!MiniValidator.TryValidate(refreshTokenDto, out var errors))
               //    return Results.ValidationProblem(errors);
               // Responsabilidade da Application 

               var token = await _authService.RefreshToken(refreshTokenDto.RefreshToken);

               return token.Succeeded ? Results.Ok(token) : Results.BadRequest(token);

           })
           .WithTags("Authentication")
           .Produces<ICommandResult<TokenDto?>>()
           .WithName("Authentication.Refresh");
    }
}
