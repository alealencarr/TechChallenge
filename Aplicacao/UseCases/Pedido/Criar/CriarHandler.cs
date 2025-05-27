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
                Domain.Entidades.Cliente? clienteBase = null;

                if (command.ClienteId.HasValue)
                {
                    clienteBase = await _clienteRepository.GetClientePorId(command.ClienteId.Value.ToString());
                }

                var pedido = new Domain.Entidades.Pedido(clienteBase);

                foreach (var item in command.Itens)
                {
                    var product = await _produtoRepository.BuscarPorID(item.ProdutoId.ToString()) ?? throw new ArgumentException($"Produto com ID {item.ProdutoId.ToString()} não encontrado.");

                    var ingredientes = new List<IngredientePersonalizado>();

                    if (product.Categoria.IsLanche())
                    {
                        foreach (var ingrediente in item.IngredientesPersonalizados)
                        {
                            var ingredienteDb = await _ingredienteRepository.GetById(ingrediente.Id.ToString()) ?? throw new ArgumentException($"Ingrediente com ID {ingrediente.Id.ToString()} não encontrado.");

                            ingredientes.Add(new IngredientePersonalizado
                            {
                                Id = ingrediente.Id,
                                Adicional = ingrediente.Adicional,
                                Preco = ingredienteDb.Preco
                            });
                        }
                    }

                    var itemPedido = new ItemPedido(pedido.Id, product.Preco, ingredientes);
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
            catch (Exception ex)
            {
                return new Contracts.Response<PedidoDTO?>(null, HttpStatusCode.InternalServerError, "Não foi possível criar o pedido.");
            }
        }



    }
}
