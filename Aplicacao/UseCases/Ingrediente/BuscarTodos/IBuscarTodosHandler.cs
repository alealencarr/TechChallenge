using Contracts.DTO.Ingrediente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Ingrediente.BuscarTodos
{
    public interface IBuscarTodosHandler
    {
        Task<Contracts.Response<List<IngredienteDTO>>> Handle();
    }
}
