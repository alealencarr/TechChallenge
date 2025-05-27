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
        private readonly BuscarTodosHandler _buscarTodosHandler;
        private readonly BuscarPorIdHandler _buscarPorIdHandler;

        public CategoriaController(BuscarPorIdHandler buscarPorIdHandler, BuscarTodosHandler buscarTodosHandler)
        {
            _buscarPorIdHandler = buscarPorIdHandler;
            _buscarTodosHandler = buscarTodosHandler;
        }

        [HttpGet(Name = "Buscar todas as categorias.")]
        [Description("Busca todas as categorias cadastradas.")]
        public async Task<IActionResult> BuscarTodos()
        {
            var result = await _buscarTodosHandler.Handler();

            return result.IsSucess ? Ok(result) : BadRequest(result);
        }


        [HttpGet(Name = "Buscar categoria por Id")]
        [Description("Busca a categoria com base no ID informado via QueryString.")]
        public async Task<IActionResult> BuscarPorId([FromQuery][Required(ErrorMessage = "Id é obrigatório.")] string id)
        {

            var result = await _buscarPorIdHandler.Handler(id);

            return result.IsSucess ? Ok(result) : BadRequest(result);


        }



    }
}
