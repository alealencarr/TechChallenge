using Domain.Entities;
using Shared.DTO.Categorie.Output;
using Shared.Result;

namespace Application.Presenter.Categories
{
    public class CustomerPresenter
    {
        private string _message = string.Empty;
        public CustomerPresenter(string message) { _message = message; }
        public ICommandResult<CustomerOutputDto> TransformObject(Customer customer)
        {
            return CommandResult<CustomerOutputDto>.Success(Transform(customer),_message);
        }

        public ICommandResult<List<CustomerOutputDto>> TransformList(List<Customer> categories)
        {
            return CommandResult<List<CustomerOutputDto>>.Success(categories.Select(x => Transform(x)).ToList());
        }

        public CustomerOutputDto Transform(Customer customer, bool conflict = false)
        {
            return new CustomerOutputDto { Id = customer.Id, Cpf = customer.Cpf!.Valor.ToString(), Name = customer.Name, Mail =  customer.Mail };
        }

        public ICommandResult<T> Error<T>(string message)
        {
            return CommandResult<T>.Fail(message);
        }

        public ICommandResult<T> Conflict<T>(string message)
        {
            return CommandResult<T>.ConflictReturn(message);
        }

    }
}
