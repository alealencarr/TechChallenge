using Aplicacao.Common;
using Aplicacao.UseCases.Produtos.Alterar;
using Aplicacao.UseCases.Produtos.Buscar;
using Aplicacao.UseCases.Produtos.Criar;
using Aplicacao.UseCases.Produtos.Remover;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Adapters.Inbound.Controllers
{
    [ApiController]
    [Route("api/produto")]
    public class ProdutoController : ControllerBase
    {

        private readonly CriarHandler _criarHandler;
        private readonly AlterarHandler _alterarHandler;
        private readonly BuscarHandler _buscarHandler; 
        private readonly RemoverHandler _removerHandler;
        public ProdutoController(CriarHandler criarHandler, AlterarHandler alterarHandler, BuscarHandler buscarHandler, RemoverHandler removerHandler)
        {
            _criarHandler = criarHandler;
            _alterarHandler = alterarHandler;
            _buscarHandler = buscarHandler;
            _removerHandler = removerHandler;
        }

        [HttpPost("criar",Order = 1)]
        [Description("Inclusão do produto com base no objeto informado via Body")]
        public async Task<IActionResult> Criar(Contracts.Request.Produto.CriarRequest request)
        {
            CriarCommand command = new(request.Nome, request.Preco, request.CategoriaId, request.Imagens, request.Descricao, request.Ingredientes);

            var result = await _criarHandler.Handle(command);

            return result.IsSucess ?
                Created($"/{result.Data?.Id}", result) :
                BadRequest(result);
        }

        [HttpPut("alterar", Order = 2)]
        [Description("Alteração do produto com base no Id informado via QueryString")]
        public async Task<IActionResult> Alterar(Contracts.Request.Produto.AlterarRequest request, [FromQuery][Required(ErrorMessage = "Id é obrigatório.")] string id)
        {

            AlterarPorIdCommand commandId = new AlterarPorIdCommand(request.Nome, request.Preco, request.CategoriaId, request.Imagens, request.Descricao,id, request.Ingredientes);
            
            var result = await _alterarHandler.Handle(commandId);

            return result.IsSucess ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpGet("filtrarPorCategoria", Order = 3)]
        [Description("Buscar produtos com base na Categoria informado na QueryString")]
        public async Task<IActionResult> Buscar([FromQuery] string? id = null, [FromQuery]  string? name = null)
        {
            var result = await _buscarHandler.Handle(id, name);

            return result.IsSucess ?
           Ok(result) :
           BadRequest(result);
        }

        [HttpDelete("{id}", Order = 4)]
        [Description("Deletar produto com base no Id.")]

        public async Task<IActionResult> Remover([FromRoute][Required(ErrorMessage = "Id é obrigatório.")] string id)
        {
            var result = await _removerHandler.Handle(id);

            return result.IsSucess ?
                Ok(result) :
                BadRequest(result);

        }


    }
}
