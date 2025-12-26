using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Result;

namespace API.Endpoints.Users;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/users/{id}",
           async (IUserService _userService, [FromRoute] Guid id) =>
           {
               var user = await _userService.Delete(id);

               return user.Succeeded ? Results.NoContent() : Results.BadRequest(user);

           })
           .WithTags("Users")
           .Produces<ICommandResult>()
           .WithName("User.Delete")
           .RequireAuthorization(new AuthorizeAttribute { Roles = "Master" });

    }
}

