using Application.Interfaces.DataSources;
using Application.UseCases.Customers.Command;
using Domain.Entities;
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

        public async Task<Customer?> GetByCpf(string cpf)
        {
            var customer = await _dataSource.GetByCpf(cpf);

            return customer is not null ? new Customer(customer.Id, customer.CreatedAt, customer.Cpf, customer.Name, customer.Mail, customer.CustomerIdentified) : null;
        }

        public async Task<Customer?> GetById(Guid id)
        {
            var customer = await _dataSource.GetById(id);

            return customer is not null ? new Customer(customer.Id, customer.CreatedAt, customer.Cpf, customer.Name, customer.Mail, customer.CustomerIdentified) : null;
        }
        public async Task CreateCustomer(Customer customer)
        {
            var customerInput = new CustomerInputDto(customer.Id, customer.CreatedAt, customer.Cpf!.Valor, customer.Name, customer.Mail, customer.CustomerIdentified);

            await _dataSource.Create(customerInput);
        }

        public async Task UpdateCustomer(Customer customer)
        {
            var customerInput = new CustomerInputDto(customer.Id, customer.CreatedAt, customer.Cpf!.Valor, customer.Name, customer.Mail, customer.CustomerIdentified);

            await _dataSource.Update(customerInput);
        }

    }
}
