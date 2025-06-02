using Aplicacao.UseCases.Categoria.BuscarPorId;
using Aplicacao.UseCases.Categoria.BuscarTodos;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Adapters.Inbound.Controllers
{
    [ApiController]
    [Route("api/categoria")]
    public class CategoriaController : ControllerBase
    {
        private readonly IBuscarTodosHandler _buscarTodosHandler;
        private readonly IBuscarPorIdHandler _buscarPorIdHandler;

        public CategoriaController(IBuscarPorIdHandler buscarPorIdHandler, IBuscarTodosHandler buscarTodosHandler)
        {
            _buscarPorIdHandler = buscarPorIdHandler;
            _buscarTodosHandler = buscarTodosHandler;
        }

        [HttpGet]
        [Description("Busca todas as categorias cadastradas.")]
        public async Task<IActionResult> BuscarTodos()
        {
            var result = await _buscarTodosHandler.Handler();

            return result.IsSucess ? Ok(result) : BadRequest(result);
        }


        [HttpGet("{id}")]
        [Description("Busca a categoria com base no ID informado.")]
        public async Task<IActionResult> BuscarPorId([FromRoute][Required(ErrorMessage = "Id é obrigatório.")] Guid id)
        {

            var result = await _buscarPorIdHandler.Handler(id.ToString());

            return result.IsSucess ? Ok(result) : BadRequest(result);


        }



    }
}
