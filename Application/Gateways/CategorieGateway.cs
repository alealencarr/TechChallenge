using Application.Interfaces.DataSources;
using Domain.Entities;
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
        public async Task<List<Categorie>> GetAll()
        {
            var categoriesDto = await _dataSource.GetAllCategories();

            return categoriesDto.Select(x => new Categorie(x.Name,  x.Id , x.CreatedAt, x.IsEditavel)).ToList();
        }

        public async Task  Delete(Guid id)
        {
            await _dataSource.Delete(id);
        }
        

        public async Task<Categorie?> GetById(Guid id)
        {
            var categorie = await _dataSource.GetCategorieById(id);

            return categorie is not null ? new Categorie(categorie.Name, categorie.Id , categorie.CreatedAt, categorie.IsEditavel) : null;
        }

        public async Task CreateCategorie(Categorie categorie)
        {
            var categorieInput = new CategorieInputDto(categorie.Id, categorie.Name, categorie.IsEditavel, categorie.CreatedAt);

            await _dataSource.CreateCategorie(categorieInput);
        }

        public async Task UpdateCategorie(Categorie categorie)
        {
            var categorieInput = new CategorieInputDto(categorie.Id, categorie.Name, categorie.IsEditavel, categorie.CreatedAt);

            await _dataSource.UpdateCategorie(categorieInput);
        }
        public async Task<Categorie?> GetByName(string name)
        {
            var categorie = await _dataSource.GetByName(name);

            return categorie is not null ? new Categorie(categorie.Name, categorie.Id, categorie.CreatedAt, categorie.IsEditavel) : null;
        }
    }
}
