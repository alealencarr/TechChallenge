using Application.Interfaces.DataSources;
using Domain.Entities;

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
        public async Task<List<Categorie>> GetAll()
        {
            var categoriesDto = await _dataSource.GetAllCategories();

            return categoriesDto.Select(x => new Categorie(x.Name,  x.Id , x.CreatedAt)).ToList();
        }

        public async Task<Categorie?> GetById(Guid id)
        {
            var categorie = await _dataSource.GetCategorieById(id);

            return categorie is not null ? new Categorie(categorie.Name, categorie.Id , categorie.CreatedAt) : null;
        }

    }
}
