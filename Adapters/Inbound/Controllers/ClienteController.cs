using Aplicacao.UseCases.Cliente.Alterar;
using Aplicacao.UseCases.Cliente.BuscarPorCPF;
using Aplicacao.UseCases.Cliente.Criar;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        public async Task<IActionResult> Criar(Contracts.Request.Cliente.CriarRequest request)
        {
            CriarCommand command = new(request.Email, request.Nome, request.Email);

            var result = await _criarHandler.Handler(command);

            return result.IsSucess ?
                Created($"/{result.Data?.Id}", result) :
                BadRequest(result);
        }

        [HttpPut("alterar", Name = "Alterar")]
        [Description("Alteração do cliente com base no CPF informado via QueryString")]
        public async Task<IActionResult> Alterar(Contracts.Request.Cliente.AlterarRequest request, [FromQuery][Required(ErrorMessage = "CPF é obrigatório.")] string cpf)
        {
            AlterarPorCPFCommand commandCpf = new AlterarPorCPFCommand(cpf, request.Nome, request.Email);

            var result = await _alterarClienteHandler.Handler(commandCpf);

            return result.IsSucess ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpGet(Name = "BuscarPorCPF")]
        [Description("Buscar o cliente com base no CPF informado via QueryString")]
        public async Task<IActionResult> BuscarPorCPF([FromQuery][Required(ErrorMessage = "CPF é obrigatório.")] string cpf)
        {
            BuscarPorCPFCommand command = new(cpf);

            var result = await _buscarPorCPFHandler.Handler(command);

            return result.IsSucess ?
           Ok(result) :
           BadRequest(result);
        }


    }
}
