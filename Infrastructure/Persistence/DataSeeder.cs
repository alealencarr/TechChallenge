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

                var userMock = new UserDbModel(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Alexandre Alencar", mailToAdm, _passwordService.HashPassword("123456789"), userRoles, null, null);

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
 
    }
}
