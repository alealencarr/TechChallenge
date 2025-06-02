using Contracts.DTO.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Cliente.BuscarPorCPF
{
    public interface IBuscarPorCPFHandler
    {
        Task<Contracts.Response<ClienteDTO?>> Handler(BuscarPorCPFCommand command);
    }
}
