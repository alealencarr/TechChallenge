using Contracts.DTO.Ingrediente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Ingrediente.Alterar
{
    public interface IAlterarHandler
    {
        Task<Contracts.Response<IngredienteDTO?>> Handler(AlterarPorIdCommand command);
    }
}
