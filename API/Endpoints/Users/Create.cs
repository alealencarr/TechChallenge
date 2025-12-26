using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Shared.DTO.User.Output;
using Shared.DTO.User.Request;
using Shared.Result;

namespace API.Endpoints.Users
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/users",
               async (IUserService _userService, [FromBody] UserRequestDto userDto) =>
               {
                   //if (!MiniValidator.TryValidate(userDto, out var errors))
                   //    return Results.ValidationProblem(errors); 
                   // Responsabilidade da Application 

                   var user = await _userService.Create(userDto);

                   return user.Succeeded ? Results.Created($"/{user.Data?.Id}", user) : Results.BadRequest(user);

               })
               .WithTags("Users")
               .Produces<ICommandResult<UserOutputDto?>>()
               .WithName("Users.Create")
               .RequireAuthorization(new AuthorizeAttribute { Roles = "Master" });

        }
    }
}

