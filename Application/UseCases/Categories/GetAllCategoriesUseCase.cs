using Application.Gateways;
using Domain.Entities;

namespace Application.UseCases.Categories
{
    public class GetAllCategoriesUseCase
    {
        CategorieGateway _gateway = null;
        public static GetAllCategoriesUseCase Create(CategorieGateway gateway)
        {
            return new GetAllCategoriesUseCase(gateway);
        }

        private GetAllCategoriesUseCase(CategorieGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<List<Categorie>> Run()
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
