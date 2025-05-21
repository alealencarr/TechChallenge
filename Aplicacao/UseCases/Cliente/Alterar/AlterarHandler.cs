using Aplicacao.Common;
using Aplicacao.UseCases.Cliente.Criar;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Cliente.Alterar
{
    public class AlterarHandler
    {
        private readonly IClienteRepository _clienteRepository;

        public AlterarHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Response<ClienteDTO?>> Handler(AlterarPorCPFCommand command)
        {

            try
            {
                var cliente = await _clienteRepository.GetClientePorCPF(command.Cpf);

                if (cliente is null)
                    return new Response<ClienteDTO?>(data: null, code: HttpStatusCode.BadRequest, "Cliente não encontrado com base neste CPF.");


                cliente.Email = command.Email;
                cliente.Nome = command.Nome;

                await _clienteRepository.Alterar(cliente);

                ClienteDTO clienteDto = new ClienteDTO(cliente.CPF.Valor, cliente.Nome, cliente.Email, cliente.Id);

                return new Response<ClienteDTO?>(data: clienteDto, code: System.Net.HttpStatusCode.OK, "Cliente alterado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Response<ClienteDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return new Response<ClienteDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível alterar o cliente.");
            }
        }
    }
}
