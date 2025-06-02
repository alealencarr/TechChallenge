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

namespace Aplicacao.UseCases.Pedido.Finalizar
{
    public class FinalizarHandler : IFinalizarHandler
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IPagamentoService _pagamentoService;
        private readonly IQRCodeService _qrCodeService;
        private readonly IFileSaver _fileSaver;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FinalizarHandler(IPedidoRepository pedidoRepository, IPagamentoService pagamentoService, IFileSaver fileSaver, IQRCodeService qrCodeService, IHttpContextAccessor httpContextAccessor)
        {
            _pedidoRepository = pedidoRepository;
            _pagamentoService = pagamentoService;
            _fileSaver = fileSaver;
            _qrCodeService = qrCodeService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Contracts.Response<QRCodeDTO?>> Handle(string id)
        {
            try
            {
                var pedido = await _pedidoRepository.GetById(id);

                if (pedido is null)
                    return new Contracts.Response<QRCodeDTO?>(data: null, code: HttpStatusCode.BadRequest, $"Pedido não encontrado com base neste Id: {id}.");

               if (pedido.StatusPedido == Domain.Entidades.Enums.EStatusPedido.Recebido)
                    return new Contracts.Response<QRCodeDTO?>(data: null, code: HttpStatusCode.BadRequest, $"Este pedido já foi pago e recebido pela cozinha.");


                var qrCodeString = await _pagamentoService.PagamentoQRCodeFakeAsync(pedido.Valor);
                var qrBytes = await _qrCodeService.GerarQrCodeAsync(qrCodeString);

                var fileName = $"pedido-{id}.png";
                await _fileSaver.SalvarArquivo(qrBytes, fileName, "qrcodes");
                
                var publicUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/qrcodes/{fileName}";

                QRCodeDTO qrCodeDto = new QRCodeDTO(id, publicUrl, $"data:image/png;base64,{Convert.ToBase64String(qrBytes)}");

                if (!pedido.AvancarStatus(out var mensagemErro))
                {
                    return new Contracts.Response<QRCodeDTO?>(
                        data: null,
                        code: HttpStatusCode.BadRequest,
                        message: mensagemErro);
                }

                await _pedidoRepository.AlterarStatus(pedido);

                return new Contracts.Response<QRCodeDTO?>(data: qrCodeDto, code: HttpStatusCode.OK, "QR code gerado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<QRCodeDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch
            {
                return new Contracts.Response<QRCodeDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível gerar o QR code.");
            }
        }

    }
}
