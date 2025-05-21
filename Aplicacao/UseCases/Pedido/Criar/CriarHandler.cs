using Aplicacao.Common;
using Aplicacao.UseCases.Cliente;
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

        public CriarHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        //public async Task<Response<Domain.Entidades.Pedido?>> Handler(CriarCommand command)
        //{
        //    //try
        //    //{
        //        var pedido = new Domain.Entidades.Pedido(command.Lanches, command.Complementos);

        //        await _pedidoRepository.Adicionar(pedido);

            //    var clienteDto = new ClienteDTO(
            //         command.Nome,
            //         command.CPF,
            //         command.Email,
            //         cliente.Id);

            //    return new Response<ClienteDTO?>(data: clienteDto, code: System.Net.HttpStatusCode.Created, "Cliente Criado com sucesso.");
            //}
            //catch (ArgumentException ex)
            //{
            //    return new Response<ClienteDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            //}
            //catch (Exception ex)
            //{
            //    return new Response<ClienteDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível criar o cliente.");
            //}
            //}

        
    }
}
