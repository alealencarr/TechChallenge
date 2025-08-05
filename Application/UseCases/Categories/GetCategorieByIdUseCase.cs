using Application.Gateways;
using Domain.Entities;

namespace Application.UseCases.Categories
{
    public class GetCategorieByIdUseCase
    {
        CategorieGateway _gateway = null;
        public static GetCategorieByIdUseCase Create(CategorieGateway gateway)
        {
            return new GetCategorieByIdUseCase(gateway);
        }

        private GetCategorieByIdUseCase(CategorieGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Categorie?> Run(Guid id)
        {
            try
            {
                var categorie = await _gateway.GetById(id);

                return categorie;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
