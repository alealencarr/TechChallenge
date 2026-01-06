using Application.Interfaces.DataSources;
using Shared.DTO.Categorie;
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
        public async Task<List<CategorieDto>> GetAllCategories()
        {
            throw new Exception();
        }

        public async Task<CategorieDto?> GetByName(string name)
        {
            return await _httpClient.GetFromJsonAsync<CategorieDto>($"api/categories/{name}");
        }

        public async Task<CategorieDto?> GetCategorieById(Guid id)
        {

            throw new Exception();
        }
    }
}
