using Contracts.DTO.Cliente;
using Domain.Ports;
using System.Net;

namespace Aplicacao.UseCases.Cliente.BuscarPorCPF
{
    public class BuscarPorCPFHandler
    {
        private readonly IClienteRepository _clienteRepository;
        
        public BuscarPorCPFHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Contracts.Response<ClienteDTO?>> Handler(BuscarPorCPFCommand command)
        {
            try
            {
                var cliente = await _clienteRepository.GetByCPF(command.CPF);

                if (cliente is null)
                    return new Contracts.Response<ClienteDTO?>(data: null, code: System.Net.HttpStatusCode.NotFound, "Cliente não encontrado.");

                ClienteDTO clienteDto = new ClienteDTO(cliente.CPF!.Valor, cliente.Nome, cliente.Email, cliente.Id);

                return new Contracts.Response<ClienteDTO?>(data: clienteDto, code: System.Net.HttpStatusCode.OK, "Cliente encontrado com sucesso!");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<ClienteDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch  
            {
                return new Contracts.Response<ClienteDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível localizar o cliente.");
            }
        }

    }
}
