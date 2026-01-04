using Application.Interfaces.DataSources;
using Shared.DTO.Ingrendient.Input;

namespace Application.Gateways
{
    public class IngredientGateway
    {
        private IIngredientDataSource _dataSource;

        private IngredientGateway(IIngredientDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        public static IngredientGateway Create(IIngredientDataSource dataSource)
        {
            return new IngredientGateway(dataSource);
        }

        public async Task<List<IngredientInputDto>> GetAll()
        {
            return await _dataSource.GetAll();
        }

        public async Task<List<IngredientInputDto>> GetByIds(List<Guid> ids)
        {
            return await _dataSource.GetByIds(ids);
        }

        public async Task<IngredientInputDto?> GetById(Guid id)
        {
            var ingredient = await _dataSource.GetById(id);

            return ingredient is not null ? ingredient  : null;
        }
 

    }
}
