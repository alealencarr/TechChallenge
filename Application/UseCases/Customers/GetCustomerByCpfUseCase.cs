using Application.Gateways;
using Domain.Entities;

namespace Application.UseCases.Customers
{
    public class GetCustomerByCpfUseCase
    {
        CustomerGateway _gateway = null;
        public static GetCustomerByCpfUseCase Create(CustomerGateway gateway)
        {
            return new GetCustomerByCpfUseCase(gateway);
        }

        private GetCustomerByCpfUseCase(CustomerGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Customer?> Run(string cpf)
        {
            try
            {
                var customer = await _gateway.GetByCpf(cpf);

                return customer;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
