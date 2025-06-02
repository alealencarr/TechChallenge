using Contracts.DTO.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Categoria.BuscarPorId
{
    public interface IBuscarPorIdHandler
    {
        Task<Contracts.Response<CategoriaDTO?>> Handler(string id);
    }
}
