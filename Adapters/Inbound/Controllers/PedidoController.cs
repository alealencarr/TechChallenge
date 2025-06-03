using Aplicacao.UseCases.Pedido.AlterarStatus;
using Aplicacao.UseCases.Pedido.BuscarPorId;
using Aplicacao.UseCases.Pedido.Criar;
using Aplicacao.UseCases.Pedido.Checkout;
using Aplicacao.UseCases.Pedido.SharedCommand;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Adapters.Inbound.Controllers
{
    [ApiController]
    [Route("api/pedido")]
    public class PedidoController : ControllerBase
    {

        private readonly ICriarHandler _criarHandler;
        private readonly ICheckoutHandler _finalizarHandler;
        private readonly IBuscarPorIdHandler _buscarPorIdHandler;
        private readonly IAlterarStatusHandler _alterarStatusHandler;
        public PedidoController(ICriarHandler criarHandler, ICheckoutHandler finalizarHandler, IBuscarPorIdHandler buscarPorIdHandler, IAlterarStatusHandler alterarStatusHandler)
        {
            _criarHandler = criarHandler;
            _finalizarHandler = finalizarHandler;
            _buscarPorIdHandler = buscarPorIdHandler;
            _alterarStatusHandler = alterarStatusHandler;
        }

        [HttpPost("criar")]
        [Description("Inclusão do Pedido com base no objeto informado via Body")]

        public async Task<IActionResult> Criar(Contracts.Request.Pedido.CriarRequest request)
        {
            CriarCommand command = new(request.ClienteId, MapRequestToCommand(request.Itens));

            var result = await _criarHandler.Handle(command);

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
                            Quantidade = ingrediente.Quantidade
                        };

                        ingredientes.Add(_ingrediente);
                    }

                }
                ItemPedidoCommand itemPedidoCommand = new ItemPedidoCommand(item.Id, ingredientes, item.Quantidade);

                itens.Add(itemPedidoCommand);
            }

            return itens;
        }

        [HttpPatch("checkoutpedido/{id}")]
        public async Task<IActionResult> Checkout([FromRoute][Required(ErrorMessage = "Favor informar o id do pedido")] Guid id)
        {
            var result = await _finalizarHandler.Handle(id.ToString());

            return result.IsSucess ? 
                Ok(result) :
                BadRequest(result);
        }

        [HttpPost("alterarstatus/{id}")]
        public async Task<IActionResult> Alterar([FromRoute][Required(ErrorMessage = "Favor informar o id do pedido")] Guid id)
        {
            var result = await _alterarStatusHandler.Handle(id.ToString());

            return result.IsSucess ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpGet("{id}")]
        [Description("Busca o pedido com base no ID informado.")]
        public async Task<IActionResult> BuscarPorId([FromRoute][Required(ErrorMessage = "Id é obrigatório.")] Guid id)
        {

            var result = await _buscarPorIdHandler.Handle(id.ToString());


            return result.IsSucess ? Ok(result) : BadRequest(result);


        }

    }
}
