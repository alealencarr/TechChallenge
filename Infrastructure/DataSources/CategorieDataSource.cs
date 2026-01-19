using Application.Interfaces.DataSources;
using Shared.DTO.Categorie;
using Shared.Result;
using System.Net.Http.Json;

namespace Infrastructure.DataSources
{
    public class CategorieDataSource : ICategorieDataSource
    {
        private readonly HttpClient _httpClient;

        public CategorieDataSource(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("CategoriesHttpClient");
        }
 

        public async Task<CategorieDto?> GetCategorieById(Guid id)
        {
            var retorno = await _httpClient.GetFromJsonAsync<CommandResult<CategorieDto>>($"api/categories/{id}");

            return retorno?.Data;
        }
    }
}
