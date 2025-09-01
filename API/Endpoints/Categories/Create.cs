﻿using Application.Controllers.Categories;
using Application.Interfaces.DataSources;
using Infrastructure.DataSources;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Shared.DTO.Categorie.Output;
using Shared.DTO.Categorie.Request;
using Shared.Result;

namespace API.Endpoints.Categories
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/categories",
               async (AppDbContext appDbContext, [FromBody] CategorieRequestDto categorieDto) =>
               {
                   if (!MiniValidator.TryValidate(categorieDto, out var errors))
                       return Results.ValidationProblem(errors);

                   ICategorieDataSource dataSource = new CategorieDataSource(appDbContext);
                   CategorieController _categorieController = new CategorieController(dataSource);
                   var categorie = await _categorieController.CreateCategorie(categorieDto);

                   return categorie.Conflict ? Results.Conflict(categorie) : categorie.Succeeded ? Results.Created($"/{categorie.Data?.Id}", categorie) : Results.BadRequest(categorie);

               })
               .WithTags("Categories")
               .Produces<ICommandResult<CategorieOutputDto?>>()
               .WithName("Categorie.Create");
        }
    }
}
