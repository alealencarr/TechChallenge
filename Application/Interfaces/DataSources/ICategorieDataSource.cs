using Shared.DTO.Categorie.Input;

namespace Application.Interfaces.DataSources
{
    public interface ICategorieDataSource
    {
        Task<List<CategorieInputDto>> GetAllCategories();

        Task<CategorieInputDto?> GetCategorieById(Guid id);
    }
}
