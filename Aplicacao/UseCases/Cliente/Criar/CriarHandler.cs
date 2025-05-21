using Aplicacao.Common;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Cliente.Criar
{
    public class CriarHandler
    {

        private readonly IClienteRepository _clienteRepository;

        public CriarHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Response<ClienteDTO?>> Handler(CriarCommand command)
        {

            try
            {
                var cliente = new Domain.Entidades.Cliente(command.CPF, command.Nome, command.Email);

                await _clienteRepository.Adicionar(cliente);

                var clienteDto = new ClienteDTO(
                     command.Nome,
                     command.CPF,
                     command.Email,
                     cliente.Id);

                return new Response<ClienteDTO?>(data: clienteDto, code: System.Net.HttpStatusCode.Created, "Cliente Criado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Response<ClienteDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return new Response<ClienteDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível criar o cliente.");
            }
        }
    }
}
