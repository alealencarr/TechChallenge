using Shared.DTO.Ingredient;

namespace Application.Interfaces.DataSources
{

    public interface IIngredientDataSource
    {
 
        Task<IngredientDto?> GetById(Guid id);
        Task<List<IngredientDto>> GetAll();

        Task<List<IngredientDto>> GetByIds(List<Guid> ids);

    }
}
