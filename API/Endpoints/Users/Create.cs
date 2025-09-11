using Application.Controllers.User;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Shared.DTO.Categorie.Output;
using Shared.DTO.Customer.Request;
using Shared.DTO.User.Request;
using Shared.Result;

namespace API.Endpoints.Users
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/user",
               async (AppDbContext appDbContext, IPasswordService passwordService,  [FromBody] UserRequestDto userDto) =>
               {
                   if (!MiniValidator.TryValidate(userDto, out var errors))
                       return Results.ValidationProblem(errors);

                   IUserDataSource dataSource = new UserDataSource(appDbContext);
                   UserController _userController = new UserController(dataSource, passwordService);
                   var user = await _userController.CreateUser(userDto);

                   return user.Succeeded ? Results.Created($"/{user.Data?.Id}", user) : Results.BadRequest(user);

               })
               .WithTags("Users")
               .Produces<ICommandResult<CustomerOutputDto?>>()
               .WithName("Users.Create")
               .RequireAuthorization(new AuthorizeAttribute { Roles = "Master" });            

        }
    }
}
