using Aplicacao.UseCases.Pedido.Criar;
using Aplicacao.UseCases.Pedido.Finalizar;
using Aplicacao.UseCases.Pedido.SharedCommand;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
 
namespace Adapters.Inbound.Controllers
{
    [ApiController]
    [Route("api/pedido")]
    public class PedidoController : ControllerBase
    {

        private readonly CriarHandler _criarHandler;
        private readonly FinalizarHandler _finalizarHandler;
        public PedidoController(CriarHandler CriarHandler, FinalizarHandler FinalizarHandler)
        {
            _criarHandler = CriarHandler;
            _finalizarHandler = FinalizarHandler;
        }

        [HttpPost("criar")]
        [Description("Inclusão do Pedido com base no objeto informado via Body")]

        public async Task<IActionResult> Criar(Contracts.Request.Pedido.CriarRequest request)
        {
            CriarCommand command = new(request.ClienteId, MapRequestToCommand(request.Itens));

            var result = await _criarHandler.Handler(command);

            return result.IsSucess ?
                Created($"/{result.Data?.Id}", result) :
                BadRequest(result);
        }

        private List<ItemPedidoCommand> MapRequestToCommand(List<Contracts.Request.Pedido.ItemPedidoRequest> itensRequest)
        {
            List<ItemPedidoCommand> itens = [];

            foreach (var item in itensRequest)
            {
                List<IngredienteCommand> ingredientes = [];
                if (item.Ingredientes != null)
                {
                    foreach (var ingrediente in item.Ingredientes)
                    {
                        IngredienteCommand _ingrediente = new IngredienteCommand()
                        {
                            Id = ingrediente.Id,
                            Adicional = ingrediente.Adicional
                        };

                        ingredientes.Add(_ingrediente);
                    }

                }
                ItemPedidoCommand itemPedidoCommand = new ItemPedidoCommand(item.Id, ingredientes);

                itens.Add(itemPedidoCommand);
            }

            return itens;
        }

        //[HttpPut("Alterar")]
        //[Description("Alteração do Pedido com base no CPF.")]
        //public async Task<IActionResult> Alterar(AlterarCommand command, [FromQuery] string cpf)
        //{
        //    AlterarPorCPFCommand commandCpf = new AlterarPorCPFCommand(cpf, command.Nome, command.Email);

        //    var result = await _alterarHandler.Handler(commandCpf);

        //    return result.IsSucess ?
        //        Ok(result) :
        //        BadRequest(result);
        //}

    }
}
