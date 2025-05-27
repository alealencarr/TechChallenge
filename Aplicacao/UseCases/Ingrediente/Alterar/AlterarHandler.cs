using Aplicacao.Common;
using Aplicacao.UseCases.Ingrediente.Criar;
using Contracts.DTO.Ingrediente;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Ingrediente.Alterar
{
    public class AlterarHandler
    {
        private readonly IIngredienteRepository _ingredienteRepository;

        public AlterarHandler(IIngredienteRepository ingredienteRepository)
        {
            _ingredienteRepository = ingredienteRepository;
        }

        public async Task<Contracts.Response<IngredienteDTO?>> Handler(AlterarPorIdCommand command)
        {

            try
            {
                var _ingrediente = await _ingredienteRepository.GetById(command.Id);

                if (_ingrediente is null)
                    return new Contracts.Response<IngredienteDTO?>(data: null, code: HttpStatusCode.BadRequest, "Ingrediente não encontrado com base neste Id.");


                _ingrediente.Preco = command.Preco;
                _ingrediente.Nome = command.Nome;

                await _ingredienteRepository.Alterar(_ingrediente);

                IngredienteDTO IngredienteDto = new(_ingrediente.Id.ToString(), _ingrediente.Preco, _ingrediente.Nome);

                return new Contracts.Response<IngredienteDTO?>(data: IngredienteDto, code: System.Net.HttpStatusCode.OK, "Ingrediente alterado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<IngredienteDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch 
            {
                return new Contracts.Response<IngredienteDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível alterar o Ingrediente.");
            }
        }
    }
}
