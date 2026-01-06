using Application.Interfaces.DataSources;
using Shared.DTO.Categorie;

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
        public async Task<List<CategorieDto>> GetAll()
        {
            return await _dataSource.GetAllCategories();

        }

        public async Task<CategorieDto?> GetById(Guid id)
        {
            var categorie = await _dataSource.GetCategorieById(id);

            return categorie is not null ? categorie : null;
        }

        public async Task<CategorieDto?> GetByName(string name)
        {
            var categorie = await _dataSource.GetByName(name);

            return categorie is not null ? categorie : null;
        }
    }
}
