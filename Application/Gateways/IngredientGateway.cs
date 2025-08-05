using Application.Interfaces.DataSources;
using Application.UseCases.Ingredients.Command;
using Domain.Entities;
using Shared.DTO.Categorie.Input;
using Shared.DTO.Ingrendient.Input;

namespace Application.Gateways
{
    public class IngredientGateway
    {
        private IIngredientDataSource _dataSource;

        private IngredientGateway(IIngredientDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        public static IngredientGateway Create(IIngredientDataSource dataSource)
        {
            return new IngredientGateway(dataSource);
        }

        public async Task<List<Ingredient>> GetAll()
        {
            var ingredients = await _dataSource.GetAll();

            return ingredients.Select(ingredient => new Ingredient(ingredient.Id, ingredient.CreatedAt, ingredient.Name, ingredient.Price)).ToList();
        }

        public async Task<List<Ingredient>> GetByIds(List<Guid> ids)
        {
            var ingredients = await _dataSource.GetByIds(ids);

            return ingredients.Select(ingredient => new Ingredient(ingredient.Id, ingredient.CreatedAt, ingredient.Name, ingredient.Price)).ToList();
        }

        public async Task<Ingredient?> GetById(Guid id)
        {
            var ingredient = await _dataSource.GetById(id);

            return ingredient is not null ? new Ingredient(ingredient.Id, ingredient.CreatedAt, ingredient.Name, ingredient.Price) : null;
        }
        public async Task CreateIngredient(Ingredient ingredient)
        {
            var ingredientInput = new IngredientInputDto(ingredient.Id, ingredient.CreatedAt, ingredient.Name, ingredient.Price );

            await _dataSource.Create(ingredientInput);
        }

        public async Task UpdateIngredient(Ingredient ingredient)
        {
            var ingredientInput = new IngredientInputDto(ingredient.Id, ingredient.CreatedAt, ingredient.Name, ingredient.Price);

            await _dataSource.Update(ingredientInput);
        }

    }
}
