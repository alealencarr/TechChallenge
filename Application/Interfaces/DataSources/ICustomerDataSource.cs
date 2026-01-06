using Shared.DTO.Categorie.Input;

namespace Application.Interfaces.DataSources
{

    public interface ICustomerDataSource
    {

        Task<CustomerDto?> GetById(Guid id);
        Task<CustomerDto?> GetByCpf(string cpf);
    }
}
