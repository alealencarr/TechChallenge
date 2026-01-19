using Application.Interfaces.DataSources;
using Shared.DTO.Categorie;
using Shared.DTO.Categorie.Input;
using Shared.Result;
using System.Net.Http.Json;

namespace Infrastructure.DataSources
{
    public class CustomerDataSource : ICustomerDataSource
    {
        private readonly HttpClient _httpClient;

        public CustomerDataSource(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("CustomersHttpClient");
        }

 
        public async Task<CustomerDto?> GetById(Guid id)
        {
            var retorno = await _httpClient.GetFromJsonAsync<CommandResult<CustomerDto>>($"api/customers/id/{id}");

            return retorno?.Data;
        }

    }
}
