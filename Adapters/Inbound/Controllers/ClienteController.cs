using Aplicacao.Common;
using Aplicacao.UseCases.Cliente.CriarCliente;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace Adapters.Inbound.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {

        private readonly CriarClienteHandler _criarClienteHandler;
 
        public ClienteController(CriarClienteHandler criarClienteHandler)
        {
            _criarClienteHandler = criarClienteHandler;
        }

        [HttpPost]
        public async Task<IResult> CriarCliente(CriarClienteCommand command)
        {    
            var result = await _criarClienteHandler.Handle(command);

            return result.IsSucess ?
                TypedResults.Created($"/{result.Data?.Id}", result) :
                TypedResults.BadRequest(result);


        }
    }
}
