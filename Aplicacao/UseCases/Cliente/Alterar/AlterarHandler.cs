using Contracts.DTO.Cliente;
using Domain.Ports;
using System.Net;

namespace Aplicacao.UseCases.Cliente.Alterar
{
    public class AlterarHandler
    {
        private readonly IClienteRepository _clienteRepository;

        public AlterarHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Contracts.Response<ClienteDTO?>> Handler(AlterarPorIdCommand command)
        {

            try
            {
                var cliente = await _clienteRepository.GetById(command.Id);

                if (cliente is null)
                    return new Contracts.Response<ClienteDTO?>(data: null, code: HttpStatusCode.BadRequest, "Cliente não encontrado com base neste Id.");


                cliente.Email = command.Email;
                cliente.Nome = command.Nome;

                await _clienteRepository.Alterar(cliente);

                ClienteDTO clienteDto = new ClienteDTO(cliente.CPF!.Valor, cliente.Nome, cliente.Email, cliente.Id);

                return new Contracts.Response<ClienteDTO?>(data: clienteDto, code: System.Net.HttpStatusCode.OK, "Cliente alterado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<ClienteDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch 
            {
                return new Contracts.Response<ClienteDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível alterar o cliente.");
            }
        }
    }
}
