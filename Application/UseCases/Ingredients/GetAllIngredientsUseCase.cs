using Application.Gateways;
using Domain.Entities;

namespace Application.UseCases.Ingredients
{

    public class GetAllIngredientsUseCase
    {
        IngredientGateway _gateway = null;
        public static GetAllIngredientsUseCase Create(IngredientGateway gateway)
        {
            return new GetAllIngredientsUseCase(gateway);
        }

        private GetAllIngredientsUseCase(IngredientGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<List<Ingredient>> Run()
        {
            try
            {
                var categories = await _gateway.GetAll();

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
