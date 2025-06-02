using Aplicacao.Common;
using Contracts.DTO.Ingrediente;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Ingrediente.BuscarPorId
{
    public class BuscarPorIdHandler : IBuscarPorIdHandler
    {
        private readonly IIngredienteRepository _ingredienteRepository;
        
        public BuscarPorIdHandler(IIngredienteRepository ingredienteRepository)
        {
            _ingredienteRepository = ingredienteRepository;
        }

        public async Task<Contracts.Response<IngredienteDTO?>> Handler(BuscarPorIdCommand command)
        {
            try
            {
                var _ingrediente = await _ingredienteRepository.GetById(command.Id);

                if (_ingrediente is null)
                    return new Contracts.Response<IngredienteDTO?>(data: null, code: System.Net.HttpStatusCode.NotFound, "Ingrediente não encontrado.");

                IngredienteDTO IngredienteDto = new IngredienteDTO(_ingrediente.Id.ToString(), _ingrediente.Preco, _ingrediente.Nome);

                return new Contracts.Response<IngredienteDTO?>(data: IngredienteDto, code: System.Net.HttpStatusCode.OK, "Ingrediente encontrado com sucesso!");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<IngredienteDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch
            {
                return new Contracts.Response<IngredienteDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível localizar o Ingrediente.");
            }
        }

    }
}
