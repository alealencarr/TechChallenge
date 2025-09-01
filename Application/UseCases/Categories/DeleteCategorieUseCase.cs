using Application.Gateways;
using Application.Interfaces.Services;

namespace Application.UseCases.Categories
{
    public class DeleteCategorieUseCase
    {
        CategorieGateway _gateway = null;

        public static DeleteCategorieUseCase Create(CategorieGateway gateway)
        {
            return new DeleteCategorieUseCase(gateway);
        }

        private DeleteCategorieUseCase(CategorieGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task Run(Guid id)
        {
            try
            {
                var categorieExists = await _gateway.GetById(id);

                if (categorieExists is null)
                    throw new Exception($"Error: Categorie not find by Id.");

                await _gateway.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
