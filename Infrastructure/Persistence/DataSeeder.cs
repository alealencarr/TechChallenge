using Domain.Entities;
using Infrastructure.DbContexts;
using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class DataSeeder
    {

        private readonly AppDbContext _context;
        private bool _seedInDb = false;
        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task Initialize()
        {
            await SeedUsers();

            if (_seedInDb)
                await _context.SaveChangesAsync();
        }


        private async Task SeedUsers()
        {
            var users = await _context.Users.FirstOrDefaultAsync();

            if (users is null)
            {
                var usersMock = new List<User>()
                {
                    new( "categories-client", "categoriesSenha!", "Categories"),
                    new( "ingredients-client", "ingredientsSenha!", "Ingredients"),
                    new( "customers-client", "customersSenha!", "Customers")
                };

                await _context.Users.AddRangeAsync(usersMock);
                _seedInDb = true;
            }
        }
    }
}
