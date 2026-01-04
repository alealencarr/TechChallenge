using Application.Interfaces.DataSources;
using Infrastructure.DbContexts;
using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Ingrendient.Input;

namespace Infrastructure.DataSources
{
    public class IngredientDataSource : IIngredientDataSource
    {
        private readonly AppDbContext _appDbContext;

        public IngredientDataSource(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

 

        public async Task<IngredientInputDto?> GetById(Guid id)
        {
            throw new Exception();

        }

        public async Task<List<IngredientInputDto>> GetAll()
        {
            throw new Exception();

        }

        public async Task<List<IngredientInputDto>> GetByIds(List<Guid> ids)
        {
            throw new Exception();

        }
    }
}
