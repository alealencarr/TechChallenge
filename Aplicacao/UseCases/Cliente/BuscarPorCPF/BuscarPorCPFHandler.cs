﻿using Aplicacao.Common;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Cliente.BuscarPorCPF
{
    public class BuscarPorCPFHandler
    {
        private readonly IClienteRepository _clienteRepository;
        
        public BuscarPorCPFHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Response<ClienteDTO?>> Handler(BuscarPorCPFCommand command)
        {
            try
            {
                var cliente = await _clienteRepository.GetClientePorCPF(command.CPF);

                ClienteDTO clienteDto = new ClienteDTO(cliente.CPF.Valor, cliente.Nome, cliente.Email, cliente.Id);

                return (cliente is null) ? new Response<ClienteDTO?>(data: null, code: System.Net.HttpStatusCode.NotFound, "Cliente não encontrado.") :
                 new Response<ClienteDTO?>(data: clienteDto, code: System.Net.HttpStatusCode.OK, "Cliente encontrado com sucesso!");
            }
            catch (ArgumentException ex)
            {
                return new Response<ClienteDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return new Response<ClienteDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível localizar o cliente.");
            }
        }

    }
}
