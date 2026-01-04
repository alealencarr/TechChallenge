using Shared.DTO.Ingrendient.Input;

namespace Application.Interfaces.DataSources
{

    public interface IIngredientDataSource
    {
 
        Task<IngredientInputDto?> GetById(Guid id);
        Task<List<IngredientInputDto>> GetAll();

        Task<List<IngredientInputDto>> GetByIds(List<Guid> ids);

    }
}
