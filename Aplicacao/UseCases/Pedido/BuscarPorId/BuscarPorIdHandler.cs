using Contracts.DTO.Cliente;
using Contracts.DTO.Pedido;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Pedido.BuscarPorId
{
    public class BuscarPorIdHandler : IBuscarPorIdHandler
    {
        private readonly IPedidoRepository _pedidoRepository;

        public BuscarPorIdHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<Contracts.Response<PedidoDTO?>> Handle(string id)
        {
            try
            {
                var pedido = await _pedidoRepository.GetById(id);

                if (pedido is null)
                    return new Contracts.Response<PedidoDTO?>(data: null, code: System.Net.HttpStatusCode.BadRequest, $"Pedido não encontrado com base neste Id: {id}.");

                PedidoDTO pedidoDTO = new PedidoDTO(pedido.Id, pedido.CreatedAt, pedido.Valor, pedido.StatusPedido.ToString(),
                    pedido.Cliente == null ? null :
                    new ClienteDTO(pedido.Cliente.CPF.Valor, pedido.Cliente.Nome,pedido.Cliente.Email, pedido.Cliente.Id) , 
                    pedido.Itens.Select(i => new ItemPedidoDTO (i.ProdutoId.ToString(), i.Quantidade, i.Preco, 
                        i.Ingredientes is null ? null : 
                        i.Ingredientes.Select(ing => new IngredienteItemPedidoDTO ( ing.IdIngrediente.ToString(), ing.Quantidade, ing.Preco, ing.Adicional ) ).ToList())).ToList()                    
                     ) ;


                return new Contracts.Response<PedidoDTO?>(data: pedidoDTO, code: System.Net.HttpStatusCode.OK, "Pedido encontrado com sucesso.");

            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<PedidoDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch
            {
                return new Contracts.Response<PedidoDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível localizar o Pedido.");
            }

        }
    }
}
