using Shared.DTO.Categorie.Input;
using Shared.DTO.Customer.Request;

namespace Application.Interfaces.DataSources
{

    public interface ICustomerDataSource
    {
        Task Create(CustomerInputDto customer);
        Task Update(CustomerInputDto customer);

        Task<CustomerInputDto?> GetById(Guid id);
        Task<CustomerInputDto?> GetByCpf(string cpf);
    }
}
