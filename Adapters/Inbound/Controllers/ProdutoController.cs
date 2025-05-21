using Aplicacao.Common;
using Aplicacao.UseCases.Produtos.Alterar;
using Aplicacao.UseCases.Produtos.Buscar;
using Aplicacao.UseCases.Produtos.Criar;
using Aplicacao.UseCases.Produtos.Remover;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
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

        [HttpPost("criar", Name = "Criar")]
        [Description("Inclusão do produto com base no objeto informado via Body")]
        public async Task<IActionResult> Criar(CriarCommand command)
        {
            var result = await _criarHandler.Handle(command);

            return result.IsSucess ?
                Created($"/{result.Data?.Id}", result) :
                BadRequest(result);
        }

        [HttpPut("alterar", Name = "Alterar")]
        [Description("Alteração do produto com base no Id informado via QueryString")]
        public async Task<IActionResult> Alterar(AlterarCommand command, [FromQuery] string id)
        {

            AlterarPorIdCommand commandId = new AlterarPorIdCommand(command.Nome, command.Preco, command.Categoria, command.Imagens, command.Descricao,id, command.Ingredientes);
            
            var result = await _alterarHandler.Handle(commandId);

            return result.IsSucess ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpPost(Name = "Buscar por Categoria")]
        [Description("Buscar produtos com base na Categoria informado no body")]
        public async Task<IActionResult> Buscar(BuscarCommand command)
        {
            var result = await _buscarHandler.Handler(command);

            return result.IsSucess ?
           Ok(result) :
           BadRequest(result);
        }


    }
}
