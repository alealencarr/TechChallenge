using Domain.Entities;
using Shared.DTO.Categorie;

namespace Application.Interfaces.DataSources
{
    public interface ICategorieDataSource
    {
        Task<List<CategorieDto>> GetAllCategories();

        Task<CategorieDto?> GetCategorieById(Guid id);

        Task<CategorieDto?> GetByName(string name);

    }
}
