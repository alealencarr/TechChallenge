using Application.Gateways;
using Domain.Entities;

namespace Application.UseCases.Ingredients
{
    public class GetIngredientByIdUseCase
    {
        IngredientGateway _gateway = null;
        public static GetIngredientByIdUseCase Create(IngredientGateway gateway)
        {
            return new GetIngredientByIdUseCase(gateway);
        }

        private GetIngredientByIdUseCase(IngredientGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Ingredient?> Run(Guid id)
        {
            try
            {
                var ingredient   = await _gateway.GetById(id);

                return ingredient;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
