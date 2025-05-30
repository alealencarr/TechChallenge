using Aplicacao.Services;
using Aplicacao.Services.Pagamento;
using Aplicacao.Services.QRCode;
using Contracts.DTO.Pedido;
using Domain.Ports;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Pedido.AlterarStatus
{
    public class AlterarStatusHandler
    {
        private readonly IPedidoRepository _pedidoRepository;
 
        public AlterarStatusHandler(IPedidoRepository pedidoRepository  )
        {
            _pedidoRepository = pedidoRepository;
         }

        public async Task<Contracts.Response<AlteracaoStatusPedidoDTO?>> Handle(string id)
        {
            try
            {
                var pedido = await _pedidoRepository.GetById(id);

                if (pedido is null)
                    return new Contracts.Response<AlteracaoStatusPedidoDTO?>(data: null, code: HttpStatusCode.BadRequest, $"Pedido não encontrado com base neste Id: {id}.");

                if (pedido.StatusPedido == Domain.Entidades.Enums.EStatusPedido.EmAberto)
                    return new Contracts.Response<AlteracaoStatusPedidoDTO?>(data: null, code: HttpStatusCode.BadRequest, $"Primeiro realize o pagamento do pedido.");


                if (!pedido.AvancarStatus(out var mensagemErro))
                {
                    return new Contracts.Response<AlteracaoStatusPedidoDTO?>(
                        data: null,
                        code: HttpStatusCode.BadRequest,
                        message: mensagemErro);
                }

                await _pedidoRepository.AlterarStatus(pedido);

                AlteracaoStatusPedidoDTO alteracao = new AlteracaoStatusPedidoDTO(pedido.Id.ToString(),  pedido.StatusPedido.ToString());

                return new Contracts.Response<AlteracaoStatusPedidoDTO?>(data: alteracao, code: HttpStatusCode.OK, "Status do pedido foi alterado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<AlteracaoStatusPedidoDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch
            {
                return new Contracts.Response<AlteracaoStatusPedidoDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível alterar o status do pedido.");
            }
        }

    }
}
