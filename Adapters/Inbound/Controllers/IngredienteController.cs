using Aplicacao.UseCases.Ingrediente.Alterar;
using Aplicacao.UseCases.Ingrediente.BuscarPorId;
using Aplicacao.UseCases.Ingrediente.Criar;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Adapters.Inbound.Controllers
{
    [ApiController]
    [Route("api/ingrediente")]
    public class IngredienteController : ControllerBase
    {

        private readonly CriarHandler _criarHandler;
        private readonly AlterarHandler _alterarIngredienteHandler;
        private readonly BuscarPorIdHandler _buscarPorIdHandler;

        public IngredienteController(CriarHandler CriarHandler, AlterarHandler alterarIngredienteHandler, BuscarPorIdHandler BuscarPorIdHandler)
        {
            _criarHandler = CriarHandler;
            _alterarIngredienteHandler = alterarIngredienteHandler;
            _buscarPorIdHandler = BuscarPorIdHandler;
        }

        [HttpPost("criar", Name = "Criar")]
        [Description("Inclusão do Ingrediente com base no objeto informado via Body")]

        public async Task<IActionResult> Criar(Contracts.Request.Ingrediente.CriarRequest request)
        {
            CriarCommand command = new(request.Preco, request.Nome);

            var result = await _criarHandler.Handler(command);

            return result.IsSucess ?
                Created($"/{result.Data?.Id}", result) :
                BadRequest(result);
        }

        [HttpPut("alterar", Name = "Alterar")]
        [Description("Alteração do Ingrediente com base no Id informado via QueryString")]
        public async Task<IActionResult> Alterar(Contracts.Request.Ingrediente.AlterarRequest request, [FromQuery][Required(ErrorMessage = "Id é obrigatório.")] string id)
        {
            AlterarPorIdCommand commandId = new AlterarPorIdCommand(id, request.Nome, request.Preco);

            var result = await _alterarIngredienteHandler.Handler(commandId);

            return result.IsSucess ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpGet(Name = "BuscarPorId")]
        [Description("Buscar o Ingrediente com base no Id informado via QueryString")]
        public async Task<IActionResult> BuscarPorId([FromQuery][Required(ErrorMessage = "Id é obrigatório.")] string id)
        {
            BuscarPorIdCommand command = new(id);

            var result = await _buscarPorIdHandler.Handler(command);

            return result.IsSucess ?
           Ok(result) :
           BadRequest(result);
        }


    }
}
