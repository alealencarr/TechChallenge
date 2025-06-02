using Contracts.DTO.Ingrediente;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Ingrediente.BuscarTodos
{
    public class BuscarTodosHandler : IBuscarTodosHandler
    {
        private readonly IIngredienteRepository _ingredienteRepository;

        public BuscarTodosHandler(IIngredienteRepository ingredienteRepository)
        {
            _ingredienteRepository = ingredienteRepository;
        }

        public async Task<Contracts.Response<List<IngredienteDTO>>> Handle()
        {
            try
            {
                var ingredientes = await _ingredienteRepository.GetAll();

                if ((ingredientes?.Count ?? 0) == 0)
                    return new Contracts.Response<List<IngredienteDTO>>(data: null, code: System.Net.HttpStatusCode.NotFound, "Nenhum ingrediente encontrado.");

                var ingredientesDto = new List<IngredienteDTO>();

                ingredientesDto = [.. ingredientes!.Select(x => new IngredienteDTO { Id = x.Id.ToString(), Nome = x.Nome, Preco = x.Preco })];


                return new Contracts.Response<List<IngredienteDTO>>(data: ingredientesDto, code: System.Net.HttpStatusCode.OK, "Ingredientes encontrados.");

            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<List<IngredienteDTO>>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch 
            {
                return new Contracts.Response<List<IngredienteDTO>>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível localizar os ingredientes.");
            }

          
        }
    }
}
