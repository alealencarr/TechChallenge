using Application.Gateways;
using Application.UseCases.Customers.Command;
using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Customers
{
 
    public class UpdateCustomerUseCase
    {
        CustomerGateway _gateway = null;
        public static UpdateCustomerUseCase Create(CustomerGateway gateway)
        {
            return new UpdateCustomerUseCase(gateway);
        }

        private UpdateCustomerUseCase(CustomerGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Customer> Run(CustomerCommand customer)
        {
            try
            {
                var customerExists = await _gateway.GetById(customer.Id);

                if (customerExists is null)
                    throw new Exception($"Error: Customer not find by Id.");

                customerExists.Cpf = new CpfVo(customer.Cpf);
                customerExists.Name = customer.Name;
                customerExists.Mail = customer.Mail;

                await _gateway.UpdateCustomer(customerExists);

                return (customerExists);
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
