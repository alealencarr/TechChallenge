using Aplicacao.Common;
using Aplicacao.UseCases.Cliente;
using Contracts.DTO.Pedido;
using Domain.Entidades.Agregados.AgregadoProduto;
using Domain.Entidades.Agregados.AgregadoPedido;
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

        public async Task<Contracts.Response<CriacaoPedidoDTO?>> Handle(CriarCommand command)
        {
            try
            {
                if (!command.Itens.Any()) //Verifica se tem algum item                  
                    return new Contracts.Response<CriacaoPedidoDTO?>(null, HttpStatusCode.BadRequest, $"Para criar um pedido é necessário pelo menos 1 item.");

                if (command.ClienteId.HasValue)//Verifica se tem o cliente foi informado
                {
                    var clienteBase = await _clienteRepository.GetById(command.ClienteId.Value.ToString());

                    if (clienteBase is null)
                        return new Contracts.Response<CriacaoPedidoDTO?>(null, HttpStatusCode.BadRequest, $"Cliente com ID {command.ClienteId.Value.ToString()} não encontrado.");

                }

                //Cria o objeto do pedido
                var pedido = new Domain.Entidades.Agregados.AgregadoPedido.Pedido(command.ClienteId);

                #region Verifica se produtos informados existem 
                List<Guid> produtosIds = command.Itens
                    .Select(i => i.ProdutoId)
                    .Distinct()
                    .ToList();

                var produtos = await _produtoRepository.GetByIds(produtosIds); 
                var produtosDict = produtos.ToDictionary(p => p.Id, p => p);
                var produtosEncontrados = produtosDict.Keys.ToHashSet();
                var produtosNaoEncontrados = produtosIds
                    .Where(id => !produtosEncontrados.Contains(id))
                    .ToList();

                if (produtosNaoEncontrados.Any())
                {
                    var idsFaltando = string.Join(", ", produtosNaoEncontrados);
                    return new Contracts.Response<CriacaoPedidoDTO?>(null, HttpStatusCode.BadRequest,
                        $"Os seguintes produtos não foram encontrados: {idsFaltando}");
                }
                #endregion

                #region Verifica se os ingredientes informados existem

                List<Guid> ingredientesIds = command.Itens
                    .SelectMany(i => i.IngredientesLanche)
                    .Select(i => i.Id)
                    .Distinct()
                    .ToList();

                var ingredientesDb = await _ingredienteRepository.GetByIds(ingredientesIds);
                var ingredientesDict = ingredientesDb.ToDictionary(i => i.Id, i => i);
                var ingredientesEncontrados = ingredientesDict.Keys.ToHashSet();
                var ingredientesNaoEncontrados = ingredientesIds
                    .Where(id => !ingredientesEncontrados.Contains(id))
                    .ToList();

                if (ingredientesNaoEncontrados.Any())
                {
                    var idsFaltando = string.Join(", ", ingredientesNaoEncontrados);
                    return new Contracts.Response<CriacaoPedidoDTO?>(null, HttpStatusCode.BadRequest,
                        $"Os seguintes ingredientes não foram encontrados: {idsFaltando}");
                }

                #endregion


                foreach (var item in command.Itens)
                {
                    if (!produtosDict.TryGetValue(item.ProdutoId, out var product))
                        return new Contracts.Response<CriacaoPedidoDTO?>(null, HttpStatusCode.BadRequest, $"Produto com ID {item.ProdutoId} não encontrado.");

                    var itemPedido = new ItemPedido(pedido.Id, product.Id, product.Preco, item.Quantidade);

                    if (product.Categoria!.IsLanche())
                    {
                        var ingredientesAgrupados = item.IngredientesLanche
                            .GroupBy(i => i.Id)
                            .Select(group => new
                            {
                                Id = group.Key,
                                QuantidadeSolicitada = group.Sum(x => x.Quantidade)
                            });

                        var ingredientesPadrao = product.ProdutoIngredientes
                        .GroupBy(pi => pi.IngredienteId)
                        .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantidade));


                        foreach (var grupo in ingredientesAgrupados)
                        {
                            if (!ingredientesDict.TryGetValue(grupo.Id, out var ingredienteDb))
                                return new Contracts.Response<CriacaoPedidoDTO?>(null, HttpStatusCode.BadRequest, $"Ingrediente com ID {grupo.Id} não encontrado.");

                            var preco = ingredienteDb.Preco;
                            var quantidadePadrao = ingredientesPadrao.ContainsKey(grupo.Id) ? ingredientesPadrao[grupo.Id] : 0;


                            int quantidadeSolicitada = grupo.QuantidadeSolicitada;

                            int qtdPadraoUsada = Math.Min(quantidadePadrao, quantidadeSolicitada);
                            if (qtdPadraoUsada > 0)
                            {
                                var ingredienteLanchePadrao = new IngredienteLanche(
                                    grupo.Id,
                                    adicional: false,
                                    preco,
                                    itemPedido.Id,
                                    qtdPadraoUsada
                                );
                                itemPedido.AdicionarIngrediente(ingredienteLanchePadrao);
                            }

                             
                            int qtdAdicional = quantidadeSolicitada - quantidadePadrao;
                            if (qtdAdicional > 0)
                            {
                                var ingredienteLancheAdicional = new IngredienteLanche(
                                    grupo.Id,
                                    adicional: true,
                                    preco,
                                    itemPedido.Id,
                                    qtdAdicional
                                );
                                itemPedido.AdicionarIngrediente(ingredienteLancheAdicional);
                            }


                        }

                        if (!itemPedido.Ingredientes!.Any())
                            return new Contracts.Response<CriacaoPedidoDTO?>(null, HttpStatusCode.BadRequest, "Um lanche deve ter ao menos um ingrediente.");
                    }

                    pedido.AdicionarItem(itemPedido);
                }

                await _pedidoRepository.Adicionar(pedido);

                var pedidoDto = new CriacaoPedidoDTO
                {
                    Id = pedido.Id,
                    ValorPedido = pedido.Valor,
                    StatusPedido = pedido.StatusPedido.ToString()
                };

                return new Contracts.Response<CriacaoPedidoDTO?>(pedidoDto, HttpStatusCode.Created, "Pedido criado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<CriacaoPedidoDTO?>(null, HttpStatusCode.BadRequest, ex.Message);
            }
            catch  
            {
                return new Contracts.Response<CriacaoPedidoDTO?>(null, HttpStatusCode.InternalServerError, "Não foi possível criar o pedido.");
            }
        }



    }
}
