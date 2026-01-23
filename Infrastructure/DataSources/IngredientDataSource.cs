using Application.Interfaces.DataSources;
using Shared.DTO.Ingredient;
using Shared.Result;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace Infrastructure.DataSources
{
    [ExcludeFromCodeCoverage]
    public class IngredientDataSource : IIngredientDataSource
    {

        private readonly HttpClient _httpClient;

        public IngredientDataSource(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("IngredientsHttpClient");
        }


        public async Task<IngredientDto?> GetById(Guid id)
        {
            var retorno = await _httpClient.GetFromJsonAsync<CommandResult<IngredientDto>>($"api/ingredients/{id}");

            return retorno?.Data;
        }


        public async Task<List<IngredientDto>?> GetByIds(List<Guid> ids)
        {
            var retornoApi = await _httpClient.PostAsJsonAsync($"api/ingredients/listIngredients", ids);

            var retornoJson = await retornoApi.Content.ReadFromJsonAsync<CommandResult<List<IngredientDto>>>();

            return retornoJson?.Data;
        }
    }
}
