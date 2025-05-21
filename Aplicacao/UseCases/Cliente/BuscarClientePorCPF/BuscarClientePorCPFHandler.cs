using Aplicacao.Common;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Cliente.BuscarClientePorCPF
{
    public class BuscarClientePorCPFHandler
    {
        private readonly IClienteRepository _clienteRepository;
        
        public BuscarClientePorCPFHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Response<Domain.Entidades.Cliente>> Handle(BuscarClientePorCPFCommand command)
        {
            try
            {
                var cliente = await _clienteRepository.GetClientePorCPF(command.CPF);

                return (cliente is null) ? new Response<Domain.Entidades.Cliente>(data: null, code: System.Net.HttpStatusCode.NotFound, "Cliente não encontrado.") :
                 new Response<Domain.Entidades.Cliente>(data: cliente, code: System.Net.HttpStatusCode.OK, "Cliente encontrado com sucesso!");
            }
            catch
            {
                return new Response<Domain.Entidades.Cliente>(data: null, code: System.Net.HttpStatusCode.InternalServerError, "Não foi possível localizar o cliente.");
            }
        }

    }
}
