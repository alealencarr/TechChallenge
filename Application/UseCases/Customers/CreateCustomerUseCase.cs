using Application.Gateways;
using Application.UseCases.Customers.Command;
using Domain.Entities;
using Shared.DTO.Customer.Request;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Customers
{
    public class CreateCustomerUseCase
    {
        CustomerGateway _gateway = null;
        public static CreateCustomerUseCase Create(CustomerGateway gateway)
        {
            return new CreateCustomerUseCase(gateway);
        }

        private CreateCustomerUseCase(CustomerGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<(Customer, bool)> Run(CustomerCommand customer)
        {
            try
            {
                var customerExists = await _gateway.GetByCpf(customer.Cpf);

                if (customerExists is not null)
                    return (customerExists, true);

                var customerEntity = new Customer(customer.Cpf, customer.Name, customer.Mail);

                await _gateway.CreateCustomer(customerEntity);

                return (customerEntity,false);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
