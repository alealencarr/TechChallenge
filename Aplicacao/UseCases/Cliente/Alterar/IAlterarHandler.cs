using Contracts.DTO.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Cliente.Alterar
{
    public interface IAlterarHandler
    {
        Task<Contracts.Response<ClienteDTO?>> Handler(AlterarPorIdCommand command);
    }
}
