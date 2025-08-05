using Application.Gateways;
using Application.UseCases.Ingredients.Command;
using Domain.Entities;

namespace Application.UseCases.Ingredients
{
    public class CreateIngredientUseCase
    {
        IngredientGateway _gateway = null;
        public static CreateIngredientUseCase Create(IngredientGateway gateway)
        {
            return new CreateIngredientUseCase(gateway);
        }

        private CreateIngredientUseCase(IngredientGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Ingredient> Run(IngredientCommand ingredient)
        {
            try
            {
                var ingredientEntity = new Ingredient(ingredient.Name, ingredient.Price);

                await _gateway.CreateIngredient(ingredientEntity);

                return ingredientEntity;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
