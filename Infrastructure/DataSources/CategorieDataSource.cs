using Application.Interfaces.DataSources;
using Domain.Entities;
using Infrastructure.DbContexts;
using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Categorie.Input;
using System.Linq;


namespace Infrastructure.DataSources
{
    public class CategorieDataSource : ICategorieDataSource
    {
        private readonly AppDbContext _appDbContext;

        public CategorieDataSource(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
 
        public async Task CreateCategorie(CategorieInputDto categorie)
        {
            var categorieDbModel = new CategorieDbModel(categorie.Id, categorie.Name, categorie.IsEditavel, categorie.CreatedAt);

            await _appDbContext.AddAsync(categorieDbModel);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateCategorie(CategorieInputDto categorie)
        {
            var categorieDb = await _appDbContext.Categories.Where(x => x.Id == categorie.Id).FirstOrDefaultAsync() ?? throw new Exception("Customer not find by Id.");
            categorieDb.Name = categorie.Name;
            categorieDb.IsEditavel = categorie.IsEditavel;

            _appDbContext.Update(categorieDb);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task<List<CategorieInputDto>> GetAllCategories()
        {
            var categories = await _appDbContext.Categories.AsNoTracking().ToListAsync();

            return categories.Select(x => new CategorieInputDto(x.Id , x.Name, x.IsEditavel, x.CreatedAt)).ToList();
        }

        public async Task<CategorieInputDto?> GetByName(string name)
        {
            var categorie = await _appDbContext.Categories.AsNoTracking().Where(x => x.Name == name).FirstOrDefaultAsync();
            return categorie is not null ? new CategorieInputDto(categorie.Id, categorie.Name, categorie.IsEditavel, categorie.CreatedAt) : null;
        }

        public async Task<CategorieInputDto?> GetCategorieById(Guid id)
        {

            var categorie = await _appDbContext.Categories.AsNoTracking().Where(x => x.Id  == id).FirstOrDefaultAsync();
            return categorie is not null ? new CategorieInputDto(categorie.Id , categorie.Name, categorie.IsEditavel, categorie.CreatedAt) : null;
        }

        public async Task Delete(Guid id)
        {

            var categorieDb = await _appDbContext.Categories.Where(x => x.Id == id).FirstOrDefaultAsync() ?? throw new Exception("Categorie not find by Id.");

            _appDbContext.Categories.Remove(categorieDb);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
