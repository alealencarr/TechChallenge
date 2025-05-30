using Aplicacao.UseCases.Produto;
using Aplicacao.UseCases.Produto.Alterar;
using Aplicacao.UseCases.Produto.BuscarPorCategoria;
using Aplicacao.UseCases.Produto.BuscarPorId;
using Aplicacao.UseCases.Produto.Criar;
using Aplicacao.UseCases.Produto.Remover;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Adapters.Inbound.Controllers
{
    [ApiController]
    [Route("api/produto")]
    public class ProdutoController : ControllerBase
    {

        private readonly CriarHandler _criarHandler;
        private readonly AlterarPorIdHandler _alterarHandler;
        private readonly BuscarHandler _buscarHandler;
        private readonly RemoverHandler _removerHandler;
        private readonly BuscarPorIdHandler _buscarPorIdHandler;
        public ProdutoController(CriarHandler criarHandler, AlterarPorIdHandler alterarHandler, BuscarHandler buscarHandler, RemoverHandler removerHandler, BuscarPorIdHandler buscarPorIdHandler)
        {
            _criarHandler = criarHandler;
            _alterarHandler = alterarHandler;
            _buscarHandler = buscarHandler;
            _removerHandler = removerHandler;
            _buscarPorIdHandler = buscarPorIdHandler;
        }

        [HttpPost("criar", Order = 1)]
        [Description("Inclusão do produto com base no objeto informado via Body")]
        public async Task<IActionResult> Create(Contracts.Request.Produto.CriarRequest request)
        {
            CriarCommand command = new(request.Nome, request.Preco, request.CategoriaId, request.Imagens, request.Descricao,
                request.Ingredientes is null ? null :
                request.Ingredientes.GroupBy(i => i.Id)
                .Select(g => new IngredienteCommand
                {
                    Id = g.Key,
                    Quantidade = g.Sum(i => i.Quantidade)
                })
                .ToList());

            var result = await _criarHandler.Handle(command);

            return result.IsSucess ?
                Created($"/{result.Data?.Id}", result) :
                BadRequest(result);
        }

        [HttpPut("alterar/{id}", Order = 2)]
        [Description("Alteração do produto com base no Id.")]
        public async Task<IActionResult> Alterar(Contracts.Request.Produto.AlterarRequest request, [FromRoute][Required(ErrorMessage = "Id é obrigatório.")] Guid id)
        {

            AlterarPorIdCommand commandId = new AlterarPorIdCommand(request.Nome, request.Preco, request.CategoriaId, request.Imagens, request.Descricao, id.ToString(),
                request.Ingredientes is null ? null :
                request.Ingredientes.GroupBy(i => i.Id)
                .Select(g => new IngredienteCommand
                {
                    Id = g.Key,
                    Quantidade = g.Sum(i => i.Quantidade)
                })
                .ToList());

            var result = await _alterarHandler.Handle(commandId);

            return result.IsSucess ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpGet("{id}", Order = 3)]
        [Description("Buscar produtos com base no Id.")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _buscarPorIdHandler.Handle(id.ToString());

            return result.IsSucess ?
           Ok(result) :
           BadRequest(result);
        }

        [HttpDelete("{id}", Order = 4)]
        [Description("Deletar produto com base no Id.")]

        public async Task<IActionResult> Remover([FromRoute][Required(ErrorMessage = "Id é obrigatório.")] Guid id)
        {
            var result = await _removerHandler.Handle(id.ToString());

            return result.IsSucess ?
                Ok(result) :
                BadRequest(result);

        }

        [HttpGet(Order = 5)]
        [Description("Buscar produtos com base na categoria informado na QueryString")]
        public async Task<IActionResult> Buscar([FromQuery] Guid? idCategoria = null, [FromQuery] string? nomeCategoria = null)
        {
            var result = await _buscarHandler.Handle(idCategoria.ToString(), nomeCategoria);

            return result.IsSucess ?
           Ok(result) :
           BadRequest(result);
        }
    }
}
