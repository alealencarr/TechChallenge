using Application.Interfaces.DataSources;
using Infrastructure.DbContexts;
using Infrastructure.DbModels.UsersModelsAggregate;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Authentication.Input;

namespace Infrastructure.DataSources
{
    public class UserDataSource : IUserDataSource
    {
        private readonly AppDbContext _appDbContext;

        public UserDataSource(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<UserInputDto?> GetUserByMail(string mail)
        {
            var user = await _appDbContext.User.Include(x => x.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync(x => x.Mail.Equals(mail));
                
            if (user is null)
                return null;
            
            return new UserInputDto(user.Id, user.Name, user.Mail, user.PasswordHash, user.UserRoles.Select(x => new UserRolesInputDto { RoleId = x.RoleId, UserId = x.UserId, Role = new RoleInputDto { Id = x.Role.Id, Name = x.Role.Name } }).ToList(), user.RefreshToken, user.RefreshTokenExpiryTime); 
        }

        public async Task<UserInputDto?> GetUserById(Guid id)
        {
            var user = await _appDbContext.User.Include(x => x.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync(x => x.Id.Equals(id)) ?? throw new Exception("Usuário não encontrado com base neste Id.");
            return new UserInputDto(user.Id, user.Name, user.Mail, user.PasswordHash, user.UserRoles.Select(x => new UserRolesInputDto { RoleId = x.RoleId, UserId = x.UserId, Role = new RoleInputDto { Id = x.Role.Id, Name = x.Role.Name } }).ToList(), user.RefreshToken, user.RefreshTokenExpiryTime);
        }
        
        public async Task<UserInputDto?> GetUserByRefreshToken(string refresh)
        {
            var user = await _appDbContext.User.Include(x => x.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync(x => x.RefreshToken.Equals(refresh)) ?? throw new Exception("Usuário não encontrado com base neste RefreshToken.");

            if (user.RefreshTokenExpiryTime <  DateTime.UtcNow)
                throw new Exception("Este RefreshToken está expirado.");

            return new UserInputDto(user.Id, user.Name, user.Mail, user.PasswordHash, user.UserRoles.Select(x => new UserRolesInputDto { RoleId = x.RoleId, UserId = x.UserId, Role = new RoleInputDto { Id = x.Role.Id, Name = x.Role.Name } }).ToList(),user.RefreshToken, user.RefreshTokenExpiryTime);
        }
        
        public async Task Update(UserInputDto user)
        {
            var userDb = await _appDbContext.User.Where(x => x.Id == user.Id).FirstOrDefaultAsync() ?? throw new Exception("User not find by Id.");
            userDb.Mail = user.Mail;
            userDb.Name = user.Name;
            userDb.PasswordHash = user.Password;
            userDb.RefreshToken = user.RefreshToken;
            userDb.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;

            _appDbContext.User.Update(userDb);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Create(UserInputDto user)
        {
            var userDbModel = new UserDbModel(user.Id, user.Name, user.Mail, user.Password, user.UserRoles.Select(x => new UserRoleDbModel(user.Id, x.RoleId)).ToList(), user.RefreshToken, user.RefreshTokenExpiryTime);

            await _appDbContext.AddAsync(userDbModel);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var userDbModel = await _appDbContext.User.Where(x => x.Id == id).FirstOrDefaultAsync() ?? throw new Exception("User not find by Id.");

            _appDbContext.User.Remove(userDbModel);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
