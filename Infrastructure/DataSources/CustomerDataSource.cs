using Application.Interfaces.DataSources;
using Shared.DTO.Categorie.Input;

namespace Infrastructure.DataSources
{
    public class CustomerDataSource : ICustomerDataSource
    {
        private readonly HttpClient _httpClient;

        public CustomerDataSource(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("CustomersHttpClient");
        }


        public async Task<CustomerDto?> GetByCpf(string cpf)
        {
            throw new Exception();

        }

        public async Task<CustomerDto?> GetById(Guid id)
        {
            throw new Exception();
        }

    }
}
