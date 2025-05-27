using Aplicacao.UseCases.Ingrediente.BuscarTodos;
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
        private readonly BuscarTodosHandler _buscarTodosHandler;

        public IngredienteController(CriarHandler CriarHandler, AlterarHandler alterarIngredienteHandler, BuscarPorIdHandler buscarPorIdHandler, BuscarTodosHandler buscarTodosHandler)
        {
            _criarHandler = CriarHandler;
            _alterarIngredienteHandler = alterarIngredienteHandler;
            _buscarPorIdHandler = buscarPorIdHandler;
            _buscarTodosHandler = buscarTodosHandler;
        }

        [HttpGet]
        [Description("Buscar todos os ingredientes")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _buscarTodosHandler.Handle();

            return result.IsSucess ?
               Ok(result) :
               BadRequest(result);
        }


        [HttpPost("criar")]
        [Description("Inclusão do Ingrediente com base no objeto informado via Body")]

        public async Task<IActionResult> Criar(Contracts.Request.Ingrediente.CriarRequest request)
        {
            CriarCommand command = new(request.Preco, request.Nome);

            var result = await _criarHandler.Handler(command);

            return result.IsSucess ?
                Created($"/{result.Data?.Id}", result) :
                BadRequest(result);
        }

        [HttpPut("alterar")]
        [Description("Alteração do Ingrediente com base no Id.")]
        public async Task<IActionResult> Alterar(Contracts.Request.Ingrediente.AlterarRequest request, [FromRoute][Required(ErrorMessage = "Id é obrigatório.")] string id)
        {
            AlterarPorIdCommand commandId = new AlterarPorIdCommand(id, request.Nome, request.Preco);

            var result = await _alterarIngredienteHandler.Handler(commandId);

            return result.IsSucess ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpGet("{id}")]
        [Description("Buscar o Ingrediente com base no Id.")]
        public async Task<IActionResult> BuscarPorId([FromRoute][Required(ErrorMessage = "Id é obrigatório.")] string id)
        {
            BuscarPorIdCommand command = new(id);

            var result = await _buscarPorIdHandler.Handler(command);

            return result.IsSucess ?
           Ok(result) :
           BadRequest(result);
        }


    }
}
