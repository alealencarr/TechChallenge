using API.Endpoints;
using Application.Controllers.User;
using Application.Interfaces.DataSources;
using Infrastructure.Configurations;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Result;

namespace API.Endpoints.Users;
 
internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/users/{id}",
           async (AppDbContext appDbContext, [FromRoute] Guid id) =>
           {
               IUserDataSource dataSource = new UserDataSource(appDbContext);
               UserController _userController = new UserController(dataSource);
               var user = await _userController.DeleteUser(id);

               return user.Succeeded ? Results.NoContent() : Results.BadRequest(user);

           })
           .WithTags("Users")
           .Produces<ICommandResult>()
           .WithName("User.Delete")
           .RequireAuthorization(new AuthorizeAttribute { Roles = "Master" });

    }
}