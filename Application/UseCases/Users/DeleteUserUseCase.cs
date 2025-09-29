using Application.Gateways;
using Application.Interfaces.Services;
using Domain.Entities.Aggregates.AggregateUser;
using Shared.DTO.User.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users
{
    public class DeleteUserUseCase
    {
        UserGateway _gateway = null;
        public static DeleteUserUseCase Create(UserGateway gateway)
        {
            return new DeleteUserUseCase(gateway);
        }

        private DeleteUserUseCase(UserGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task Run(Guid id)
        {
            try
            {
                var userExists = await _gateway.GetUserById(id);

                if (userExists is null)
                    throw new Exception("Usuário não encontrado com base neste e-mail.");

                await _gateway.Delete(id);

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
