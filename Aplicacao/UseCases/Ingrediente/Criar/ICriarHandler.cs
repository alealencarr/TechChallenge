using Contracts.DTO.Ingrediente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Ingrediente.Criar
{
    public interface ICriarHandler
    {
        Task<Contracts.Response<IngredienteDTO?>> Handler(CriarCommand command);
    }
}
