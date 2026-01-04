using Application.Interfaces.DataSources;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Categorie.Input;


namespace Infrastructure.DataSources
{
    public class CategorieDataSource : ICategorieDataSource
    {
        private readonly AppDbContext _dbContext;

        public CategorieDataSource(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<CategorieInputDto>> GetAllCategories()
        {
            throw new Exception();
        }

        public async Task<CategorieInputDto?> GetByName(string name)
        {
            throw new Exception();
        }

        public async Task<CategorieInputDto?> GetCategorieById(Guid id)
        {

            throw new Exception();
        }
    }
}
