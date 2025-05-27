using Aplicacao.Common;
using Aplicacao.UseCases.Cliente;
using Contracts.DTO.Pedido;
using Domain.Entidades;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Pedido.Criar
{
    public class CriarHandler
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IIngredienteRepository _ingredienteRepository;
        private readonly IProdutoRepository _produtoRepository;
        public CriarHandler(IPedidoRepository pedidoRepository, IClienteRepository clienteRepository, IProdutoRepository produtoRepository, IIngredienteRepository ingredienteRepository)
        {
            _ingredienteRepository = ingredienteRepository;
            _pedidoRepository = pedidoRepository;
            _clienteRepository = clienteRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<Contracts.Response<PedidoDTO?>> Handler(CriarCommand command)
        {
            try
            {

                if (command.ClienteId.HasValue)
                {
                    var clienteBase = await _clienteRepository.GetById(command.ClienteId.Value.ToString());

                    if (clienteBase is null)
                        return new Contracts.Response<PedidoDTO?>(null, HttpStatusCode.BadRequest, $"Cliente com ID {command.ClienteId.Value.ToString()} não encontrado.");

                }

                var pedido = new Domain.Entidades.Pedido(command.ClienteId);

                foreach (var item in command.Itens)
                {
                    var product = await _produtoRepository.GetById(item.ProdutoId.ToString());

                    if (product is null)
                        return new Contracts.Response<PedidoDTO?>(null, HttpStatusCode.BadRequest, $"Produto com ID {item.ProdutoId.ToString()} não encontrado.");

                    var ingredientes = new List<IngredienteLanche>();

                    var itemPedido = new ItemPedido(pedido.Id, product.Id, product.Preco);

                    if (product.Categoria!.IsLanche())
                    {
                        foreach (var ingrediente in item.IngredientesLanche)
                        {
                            var ingredienteDb = await _ingredienteRepository.GetById(ingrediente.Id.ToString());

                            if (ingredienteDb is null)
                                return new Contracts.Response<PedidoDTO?>(null, HttpStatusCode.BadRequest, $"Ingrediente com ID {ingrediente.Id.ToString()} não encontrado.");

                            var _ingredienteLanche = new IngredienteLanche
                            (
                                ingrediente.Id,
                                ingrediente.Adicional,
                                ingredienteDb.Preco,
                                itemPedido.Id
                            );

                            itemPedido.AdicionarIngrediente(_ingredienteLanche);             
                        }
                    }

                    pedido.AdicionarItem(itemPedido);
                }

                await _pedidoRepository.Adicionar(pedido);

                var pedidoDto = new PedidoDTO
                {
                    Id = pedido.Id,
                    ValorPedido = pedido.PrecoPedido
                };

                return new Contracts.Response<PedidoDTO?>(pedidoDto, HttpStatusCode.Created, "Pedido criado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<PedidoDTO?>(null, HttpStatusCode.BadRequest, ex.Message);
            }
            catch  
            {
                return new Contracts.Response<PedidoDTO?>(null, HttpStatusCode.InternalServerError, "Não foi possível criar o pedido.");
            }
        }



    }
}
