using Application.Interfaces.DataSources;
using Infrastructure.DbContexts;
using Shared.DTO.Categorie.Input;

namespace Infrastructure.DataSources
{
    public class CustomerDataSource : ICustomerDataSource
    {
        private readonly AppDbContext _appDbContext;

        public CustomerDataSource(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<CustomerInputDto?> GetByCpf(string cpf)
        {
            throw new Exception();

        }

        public async Task<CustomerInputDto?> GetById(Guid id)
        {
            throw new Exception();
        }

    }
}
