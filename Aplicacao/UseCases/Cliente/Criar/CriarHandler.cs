using Contracts.DTO.Cliente;
using Domain.Ports;
using System.Net;


namespace Aplicacao.UseCases.Cliente.Criar
{
    public class CriarHandler : ICriarHandler
    {

        private readonly IClienteRepository _clienteRepository;

        public CriarHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Contracts.Response<ClienteDTO?>> Handler(CriarCommand command)
        {

            try
            {
                var clienteCadastrado = await _clienteRepository.GetByCPF(command.CPF);

                if (clienteCadastrado is not null)
                    return new Contracts.Response<ClienteDTO?>(data: new ClienteDTO(
                     clienteCadastrado.CPF.Valor,
                                   clienteCadastrado.Nome,
                     clienteCadastrado.Email,
                     clienteCadastrado.Id), code: System.Net.HttpStatusCode.OK, "Cliente já cadastrado.");

                var cliente = new Domain.Entidades.Cliente(command.CPF, command.Nome, command.Email);

                await _clienteRepository.Adicionar(cliente);

                var clienteDto = new ClienteDTO(
                     command.CPF,
                     command.Nome,
                     command.Email,
                     cliente.Id);

                return new Contracts.Response<ClienteDTO?>(data: clienteDto, code: System.Net.HttpStatusCode.Created, "Cliente Criado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<ClienteDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch
            {
                return new Contracts.Response<ClienteDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível criar o cliente.");
            }
        }
    }
}
