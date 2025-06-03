﻿using Aplicacao.UseCases.Cliente.Alterar;
using Aplicacao.UseCases.Cliente.BuscarPorCPF;
using Aplicacao.UseCases.Cliente.Criar;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Adapters.Inbound.Controllers
{
    [ApiController]
    [Route("api/cliente")]
    public class ClienteController : ControllerBase
    {

        private readonly ICriarHandler _criarHandler;
        private readonly IAlterarHandler _alterarClienteHandler;
        private readonly IBuscarPorCPFHandler _buscarPorCPFHandler;

        public ClienteController(ICriarHandler CriarHandler, IAlterarHandler alterarClienteHandler, IBuscarPorCPFHandler BuscarPorCPFHandler)
        {
            _criarHandler = CriarHandler;
            _alterarClienteHandler = alterarClienteHandler;
            _buscarPorCPFHandler = BuscarPorCPFHandler;
        }

        [HttpPost("criar")]
        [Description("Inclusão do cliente com base no objeto informado via Body")]

        public async Task<IActionResult> Criar(Contracts.Request.Cliente.CriarRequest request)
        {
            CriarCommand command = new(request.CPF, request.Nome, request.Email);

            var result = await _criarHandler.Handler(command);

            return result.IsSucess ?
                Created($"/{result.Data?.Id}", result) :
                BadRequest(result);
        }

        [HttpPut("alterar/{id}")]
        [Description("Alteração do cliente com base no Id.")]
        public async Task<IActionResult> Alterar([FromBody] Contracts.Request.Cliente.AlterarRequest request, [FromRoute][Required(ErrorMessage = "Id é obrigatório.")] Guid id)
        {
            AlterarPorIdCommand commandId = new AlterarPorIdCommand(id.ToString(), request.Nome, request.Email);

            var result = await _alterarClienteHandler.Handler(commandId);

            return result.IsSucess ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpGet("cpf/{cpf}")]
        [Description("Buscar o cliente com base no CPF.")]
        public async Task<IActionResult> BuscarPorCPF([FromRoute][Required(ErrorMessage = "CPF é obrigatório.")] string cpf)
        {
            BuscarPorCPFCommand command = new(cpf);

            var result = await _buscarPorCPFHandler.Handler(command);

            return result.IsSucess ?
           Ok(result) :
           BadRequest(result);
        }



    }
}
