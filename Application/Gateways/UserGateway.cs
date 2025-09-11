using Application.Interfaces.DataSources;
using Domain.Entities.Aggregates.AggregateUser;
using Shared.DTO.Authentication.Input;

namespace Application.Gateways
{
    public class UserGateway
    {
        private IUserDataSource _dataSource;

        private UserGateway(IUserDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        public static UserGateway Create(IUserDataSource dataSource)
        {
            return new UserGateway(dataSource);
        }

        public async Task<User?> GetUserByMail(string mail)
        {
            var user = await _dataSource.GetUserByMail(mail);

            return user is not null ? new User(user.Id, user.Name, user.Mail, user.Password, user.UserRoles.Select(x => new UserRole(x.UserId, x.RoleId, new Role(x.Role.Id, x.Role.Name) )).ToList(), user.RefreshToken, user.RefreshTokenExpiryTime) : null;
        }


        public async Task<User?> GetUserById(Guid id)
        {
            var user = await _dataSource.GetUserById(id);

            return user is not null ? new User(user.Id, user.Name, user.Mail, user.Password, user.UserRoles.Select(x => new UserRole(x.UserId, x.RoleId, new Role(x.Role.Id, x.Role.Name))).ToList(), user.RefreshToken, user.RefreshTokenExpiryTime) : null;
        }
        
        public async Task<User?> GetUserByRefreshToken(string refresh)
        {
            var user = await _dataSource.GetUserByRefreshToken(refresh);

            return user is not null ? new User(user.Id, user.Name, user.Mail, user.Password, user.UserRoles.Select(x => new UserRole(x.UserId, x.RoleId, new Role(x.Role.Id, x.Role.Name))).ToList(), user.RefreshToken, user.RefreshTokenExpiryTime) : null;
        }
        
        public async Task UpdateUser(User user)
        {
            var userInputDto = new UserInputDto(user.Id, user.Name, user.Mail, user.PasswordHash, user.UserRoles.Select(x => new UserRolesInputDto { RoleId = x.RoleId, UserId = x.UserId }).ToList(), user.RefreshToken, user.RefreshTokenExpiryTime);

            await _dataSource.Update(userInputDto);
        }

        public async Task CreateUser(User user)
        {
            var userInputDto = new UserInputDto(user.Id, user.Name, user.Mail, user.PasswordHash, user.UserRoles.Select(x => new UserRolesInputDto { RoleId = x.RoleId, UserId = x.UserId }).ToList(), user.RefreshToken, user.RefreshTokenExpiryTime);

            await _dataSource.Create(userInputDto);
        }

        public async Task Delete(Guid id)
        {
            await _dataSource.Delete(id);
        }

    }
}

