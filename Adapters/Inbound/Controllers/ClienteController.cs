using Aplicacao.Common;
using Aplicacao.UseCases.Cliente.Alterar;
using Aplicacao.UseCases.Cliente.BuscarPorCPF;
using Aplicacao.UseCases.Cliente.Criar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Adapters.Inbound.Controllers
{
    [ApiController]
    [Route("api/cliente")]
    public class ClienteController : ControllerBase
    {

        private readonly CriarHandler _criarHandler;
        private readonly AlterarHandler _alterarClienteHandler;
        private readonly BuscarPorCPFHandler _buscarPorCPFHandler;

        public ClienteController(CriarHandler CriarHandler, AlterarHandler alterarClienteHandler, BuscarPorCPFHandler BuscarPorCPFHandler)
        {
            _criarHandler = CriarHandler;
            _alterarClienteHandler = alterarClienteHandler;
            _buscarPorCPFHandler = BuscarPorCPFHandler;
        }

        [HttpPost("criar", Name = "Criar")]
        [Description("Inclusão do cliente com base no objeto informado via Body")]

        public async Task<IActionResult> Criar(CriarCommand command)
        {    
            var result = await _criarHandler.Handler(command);

            return result.IsSucess ?
                Created($"/{result.Data?.Id}", result) :
                BadRequest(result);
        }

        [HttpPut("alterar", Name = "Alterar" )]
        [Description("Alteração do cliente com base no CPF informado via QueryString")]
        public async Task<IActionResult> Alterar(AlterarCommand command, [FromQuery] string cpf)
        {
            AlterarPorCPFCommand commandCpf = new AlterarPorCPFCommand(cpf, command.Nome, command.Email);

            var result = await _alterarClienteHandler.Handler(commandCpf);

            return result.IsSucess ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpGet( Name = "BuscarPorCPF")]
        [Description("Buscar o cliente com base no CPF informado via QueryString")]
        public async Task<IActionResult> BuscarPorCPF([FromQuery] string cpf)
        {
            BuscarPorCPFCommand command = new(cpf);

            var result = await _buscarPorCPFHandler.Handler(command);
           
            return result.IsSucess ?
           Ok(result) :
           BadRequest(result);
        }


    }
}
