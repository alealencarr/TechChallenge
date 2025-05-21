using Aplicacao.Common;
using Aplicacao.UseCases.Pedido;
using Aplicacao.UseCases.Pedido.Alterar;
using Aplicacao.UseCases.Pedido.Criar;
using Aplicacao.UseCases.Pedido.Finalizar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Adapters.Inbound.Controllers
{
    [ApiController]
    [Route("api/pedido")]
    public class PedidoController : ControllerBase
    {

        private readonly CriarHandler _criarHandler;
        private readonly AlterarHandler _alterarHandler;
        private readonly FinalizarHandler _finalizarHandler;
        public PedidoController(CriarHandler CriarHandler, AlterarHandler AlterarHandler, FinalizarHandler FinalizarHandler)
        {
            _criarHandler = CriarHandler;
            _alterarHandler = AlterarHandler;
            _finalizarHandler = FinalizarHandler;
        }

        [HttpPost("criar", Name = "Criar")]
        [Description("Inclusão do Pedido com base no objeto informado via Body")]

        public async Task<IActionResult> Criar(CriarCommand command)
        {
            if (command.Lanches is null && command.Complementos is null)
                return BadRequest(new Response<PedidoDTO?>(data: null, code: System.Net.HttpStatusCode.BadRequest, message: "Para criar um pedido é necessário informar ao menos 1 Lanche ou 1 acompanhamento."));

            dynamic result = new object(); //await _criarHandler.Handler(command);

            return result.IsSucess ?
                Created($"/{result.Data?.Id}", result) :
                BadRequest(result);
        }

        //[HttpPut("Alterar", Name = "Alterar")]
        //[Description("Alteração do Pedido com base no CPF informado via QueryString")]
        //public async Task<IActionResult> Alterar(AlterarCommand command, [FromQuery] string cpf)
        //{
        //    AlterarPorCPFCommand commandCpf = new AlterarPorCPFCommand(cpf, command.Nome, command.Email);

        //    var result = await _alterarHandler.Handler(commandCpf);

        //    return result.IsSucess ?
        //        Ok(result) :
        //        BadRequest(result);
        //}

        //[HttpGet(Name = "BuscarPedidoPorCPF")]
        //[Description("Buscar o Pedido com base no CPF informado via QueryString")]
        //public async Task<IActionResult> BuscarPorCPF([FromQuery] string cpf)
        //{
        //    BuscarPedidoPorCPFCommand command = new(cpf);

        //    var result = await _buscarPedidoPorCPFHandler.Handler(command);

        //    return result.IsSucess ?
        //   Ok(result) :
        //   BadRequest(result);
        //}


    }
}
