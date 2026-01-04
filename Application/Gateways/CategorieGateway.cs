using Application.Interfaces.DataSources;
using Shared.DTO.Categorie.Input;

namespace Application.Gateways
{
    public class CategorieGateway
    {
        private ICategorieDataSource _dataSource;

        private CategorieGateway(ICategorieDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        public static CategorieGateway Create(ICategorieDataSource dataSource)
        {
            return new CategorieGateway(dataSource);
        }
        public async Task<List<CategorieInputDto>> GetAll()
        {
            return await _dataSource.GetAllCategories();

        }

        public async Task<CategorieInputDto?> GetById(Guid id)
        {
            var categorie = await _dataSource.GetCategorieById(id);

            return categorie is not null ? categorie : null;
        }

        public async Task<CategorieInputDto?> GetByName(string name)
        {
            var categorie = await _dataSource.GetByName(name);

            return categorie is not null ? categorie : null;
        }
    }
}
