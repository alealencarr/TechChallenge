using Application.Gateways;
using Application.Interfaces.Services;
using Application.UseCases.Users;
using Domain.Entities;
using Domain.Entities.Aggregates.AggregateUser;
using Shared.DTO.User.Request;

namespace Application.UseCases.Users
{
    public class CreateUserUseCase
    {
        UserGateway _gateway = null;
        IPasswordService _passwordService;
        public static CreateUserUseCase Create(UserGateway gateway, IPasswordService passwordService)
        {
            return new CreateUserUseCase(gateway, passwordService);
        }

        private CreateUserUseCase(UserGateway gateway, IPasswordService passwordService) 
        {
            _gateway = gateway;
            _passwordService = passwordService; 
        }

        public async Task<User> Run(UserRequestDto user)
        {
            try
            {
                var userExists = await _gateway.GetUserByMail(user.Mail);

                if (userExists is not null)
                    throw new Exception("Já existe um usuário com este e-mail.");

                var userEntity = new User(user.Name, user.Mail, _passwordService.HashPassword(user.Password));

                await _gateway.CreateUser(userEntity);

                return userEntity;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
