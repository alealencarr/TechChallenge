using Application.Controllers.Authentication;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Authentication.Output;
using Shared.Result;

namespace API.Endpoints.Authentication;

internal sealed class Refresh : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/authentication/refresh/",
           async (AppDbContext appDbContext, ITokenService tokenService, [FromBody] string refreshToken) =>
           {
               if (string.IsNullOrWhiteSpace(refreshToken))
                   return Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        { "refreshToken", new[] { "Favor informar o Refresh Token no body da requisição." } }
                    });

               IUserDataSource dataSource = new UserDataSource(appDbContext);
               AuthenticationController _userController = new(dataSource, tokenService, null);
               var user = await _userController.Refresh(refreshToken);

               return user.Succeeded ? Results.Ok(user) : Results.BadRequest(user);

           })
           .WithTags("Authentication")
           .Produces<ICommandResult<TokenDto?>>()
           .WithName("Authentication.Refresh");
    }
}
