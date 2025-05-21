using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Cliente.AlterarCliente
{
    public class AlterarClienteHandler
    {
        private readonly IClienteRepository _clienteRepository;

        public AlterarClienteHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        //public Task Handle(AlterarClienteCommand command)
        //{
            
        //    _clienteRepository.GetClientePorCPF(command.CPF);


        //}
    }
}
