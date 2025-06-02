using Contracts.DTO.Produto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Produto.BuscarPorCategoria
{
    public interface IBuscarHandler
    {
        Task<Contracts.Response<List<ProdutoDTO>?>> Handle(string? id, string? name);
    }
}
