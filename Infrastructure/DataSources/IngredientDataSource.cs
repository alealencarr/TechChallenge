using Application.Interfaces.DataSources;
using Shared.DTO.Ingredient;

namespace Infrastructure.DataSources
{
    public class IngredientDataSource : IIngredientDataSource
    {

        private readonly HttpClient _httpClient;

        public IngredientDataSource(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("IngredientsHttpClient");
        }


        public async Task<IngredientDto?> GetById(Guid id)
        {
            throw new Exception();

        }

        public async Task<List<IngredientDto>> GetAll()
        {
            throw new Exception();

        }

        public async Task<List<IngredientDto>> GetByIds(List<Guid> ids)
        {
            throw new Exception();

        }
    }
}
