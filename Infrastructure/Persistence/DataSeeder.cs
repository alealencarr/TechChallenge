using Application.Interfaces.Services;
using Infrastructure.DbContexts;
using Infrastructure.DbModels;
using Infrastructure.DbModels.UsersModelsAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class DataSeeder
    {

        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;
        private bool _seedInDb = false;
        public DataSeeder(AppDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task Initialize()
        {
            await SeedIngredientes();
            await SeedCategories();
            await SeedRoles();
            await SeedUserAdm();
            
            if (_seedInDb) //Salva o contexto completo
                await _context.SaveChangesAsync();
        }

        private async Task SeedUserAdm()
        {
            string mailToAdm = "ale.alencarr@outlook.com.br";

            var userDb = await _context.User.FirstOrDefaultAsync(x => x.Mail == mailToAdm);

            if (userDb is null)
            {
                var userRoles = new List<UserRoleDbModel>
                {
                    new(Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Parse("00000000-0000-0000-0000-000000000001"))
                };

                var userMock = new UserDbModel(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Alexandre Alencar", mailToAdm, _passwordService.HashPassword("123465789"), userRoles, null, null);

                await _context.User.AddAsync(userMock);
                _seedInDb = true;
            }
        }
        private async Task SeedRoles()
        {
            var rolesDb = await _context.Role.FirstOrDefaultAsync();

            if (rolesDb is null)
            {
                var rolesMock = new List<RoleDbModel>()
                {
                    new RoleDbModel(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Master"),
                    new RoleDbModel(Guid.Parse("00000000-0000-0000-0000-000000000002"), "Admin"),
                };

                await _context.Role.AddRangeAsync(rolesMock);
                _seedInDb = true;
            }
        }
        private async Task SeedCategories()
        {
            var categoriesDb = await _context.Categorie.FirstOrDefaultAsync();

            if (categoriesDb is null)
            {
                var categoriesMock = new List<CategorieDbModel>()
                {
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Lanche",true),
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000002"), "Acompanhamento",false),
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000003"), "Bebida",false),
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000004"), "Sobremesa",false)
                };

                await _context.Categorie.AddRangeAsync(categoriesMock);
                _seedInDb = true;
            }
        }

        private async Task SeedIngredientes()
        {
            var ingredientesDb = await _context.Ingredient.FirstOrDefaultAsync();

            if (ingredientesDb is null)
            {
                var ingredientesMock = new List<IngredientDbModel>()
                {
                                    // Carnes
                new IngredientDbModel(Guid.Parse("10000000-0000-0000-0000-000000000011"), "Hambúrguer Angus 200g", 10.00m),
                new IngredientDbModel(Guid.Parse("10000000-0000-0000-0000-000000000012"), "Frango Grelhado", 7.50m),
                new IngredientDbModel(Guid.Parse("10000000-0000-0000-0000-000000000013"), "Carne Desfiada BBQ", 9.00m),

                // Queijos
                new IngredientDbModel(Guid.Parse("10000000-0000-0000-0000-000000000014"), "Queijo Muçarela", 1.50m),
                new IngredientDbModel(Guid.Parse("10000000-0000-0000-0000-000000000015"), "Queijo Gorgonzola", 2.00m),

                // Extras e Vegetais
                new IngredientDbModel(Guid.Parse("10000000-0000-0000-0000-000000000016"), "Ovo Frito", 1.50m),
                new IngredientDbModel(Guid.Parse("10000000-0000-0000-0000-000000000017"), "Rúcula Fresca", 0.80m),
                new IngredientDbModel(Guid.Parse("10000000-0000-0000-0000-000000000018"), "Jalapeño", 1.00m),

                // Molhos Premium
                new IngredientDbModel(Guid.Parse("10000000-0000-0000-0000-000000000019"), "Barbecue Defumado", 1.00m),
                new IngredientDbModel(Guid.Parse("10000000-0000-0000-0000-000000000020"), "Mostarda e Mel", 1.00m),
                new IngredientDbModel(Guid.Parse("10000000-0000-0000-0000-000000000021"), "Pimenta Chipotle", 1.20m),

                // Extras Gourmet
                new IngredientDbModel(Guid.Parse("10000000-0000-0000-0000-000000000022"), "Cebola Crispy", 1.20m),
                new IngredientDbModel(Guid.Parse("10000000-0000-0000-0000-000000000023"), "Tomate Confit", 1.50m),
                new IngredientDbModel(Guid.Parse("10000000-0000-0000-0000-000000000024"), "Molho Trufado", 2.50m)
                };

                await _context.Ingredient.AddRangeAsync(ingredientesMock);
                _seedInDb = true;
            }
        }
    }
}
