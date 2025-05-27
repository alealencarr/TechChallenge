using Aplicacao.UseCases.Pedido.Alterar;
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
