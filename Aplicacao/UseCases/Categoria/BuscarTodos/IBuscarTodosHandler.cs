using Contracts.DTO.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Categoria.BuscarTodos
{
    public interface IBuscarTodosHandler 
    {
        Task<Contracts.Response<List<CategoriaDTO>>> Handler();
    }
}
