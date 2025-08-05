using Application.Interfaces.DataSources;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Categorie.Input;


namespace Infrastructure.DataSources
{
    public class CategorieDataSource : ICategorieDataSource
    {
        private readonly AppDbContext _appDbContext;

        public CategorieDataSource(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<CategorieInputDto>> GetAllCategories()
        {
            var categories = await _appDbContext.Categories.AsNoTracking().ToListAsync();

            return categories.Select(x => new CategorieInputDto(x.Id , x.Name, x.CreatedAt)).ToList();
        }

        public async Task<CategorieInputDto?> GetCategorieById(Guid id)
        {

            var categorie = await _appDbContext.Categories.AsNoTracking().Where(x => x.Id  == id).FirstOrDefaultAsync();
            return categorie is not null ? new CategorieInputDto(categorie.Id , categorie.Name, categorie.CreatedAt) : null;
        }
    }
}
