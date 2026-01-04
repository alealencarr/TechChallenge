using Application.Interfaces.DataSources;
using Shared.DTO.Categorie.Input;

namespace Application.Gateways
{
    public class CustomerGateway
    {
        private ICustomerDataSource _dataSource;

        private CustomerGateway(ICustomerDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        public static CustomerGateway Create(ICustomerDataSource dataSource)
        {
            return new CustomerGateway(dataSource);
        }

        public async Task<CustomerInputDto?> GetByCpf(string cpf)
        {
            var customer = await _dataSource.GetByCpf(cpf);

            return customer is not null ? customer : null;
        }

        public async Task<CustomerInputDto?> GetById(Guid id)
        {
            var customer = await _dataSource.GetById(id);

            return customer is not null ? customer : null;
        }


    }
}
