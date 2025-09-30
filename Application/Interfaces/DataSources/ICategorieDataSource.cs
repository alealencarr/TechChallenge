using Domain.Entities;
using Shared.DTO.Categorie.Input;

namespace Application.Interfaces.DataSources
{
    public interface ICategorieDataSource
    {
        Task<List<CategorieInputDto>> GetAllCategories();

        Task<CategorieInputDto?> GetCategorieById(Guid id);

        Task CreateCategorie(CategorieInputDto categorie);

        Task<CategorieInputDto?> GetByName(string name);

        Task UpdateCategorie(CategorieInputDto categorie);

        Task Delete(Guid id);

    }
}
