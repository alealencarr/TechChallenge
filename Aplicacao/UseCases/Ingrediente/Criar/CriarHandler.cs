using Aplicacao.Common;
using Contracts.DTO.Ingrediente;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Ingrediente.Criar
{
    public class CriarHandler
    {

        private readonly IIngredienteRepository _ingredienteRepository;

        public CriarHandler(IIngredienteRepository IngredienteRepository)
        {
            _ingredienteRepository = IngredienteRepository;
        }

        public async Task<Contracts.Response<IngredienteDTO?>> Handler(CriarCommand command)
        {

            try
            {
                var _ingrediente = new Domain.Entidades.Ingrediente(command.Nome, command.Preco);

                await _ingredienteRepository.Adicionar(_ingrediente);

                var IngredienteDto = new IngredienteDTO(
                     _ingrediente.Id.ToString(),                     
                     _ingrediente.Preco,
                     _ingrediente.Nome
                     );

                return new Contracts.Response<IngredienteDTO?>(data: IngredienteDto, code: System.Net.HttpStatusCode.Created, "Ingrediente Criado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<IngredienteDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch  
            {
                return new Contracts.Response<IngredienteDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível criar o Ingrediente.");
            }
        }
    }
}
