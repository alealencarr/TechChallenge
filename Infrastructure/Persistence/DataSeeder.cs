using Infrastructure.DbContexts;
using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class DataSeeder
    {

        private readonly AppDbContext _context;
        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task Initialize()
        {
            await SeedIngredientes();
            await SeedCategories();
        }

        private async Task SeedCategories()
        {
            var categoriesDb = await _context.Categories.FirstOrDefaultAsync();

            if (categoriesDb is null)
            {
                var categoriesMock = new List<CategorieDbModel>()
                {
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Lanche"),
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000002"), "Acompanhamento"),
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000003"), "Bebida"),
                    new CategorieDbModel(Guid.Parse("00000000-0000-0000-0000-000000000004"), "Sobremesa")
                };

                await _context.Categories.AddRangeAsync(categoriesMock);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedIngredientes()
        {
            var ingredientesDb = await _context.Ingredients.FirstOrDefaultAsync();

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

                await _context.Ingredients.AddRangeAsync(ingredientesMock);
                await _context.SaveChangesAsync();
            }
        }
    }
}
