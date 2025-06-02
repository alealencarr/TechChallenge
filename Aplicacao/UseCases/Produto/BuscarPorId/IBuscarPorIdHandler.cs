using Contracts.DTO.Produto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Produto.BuscarPorId
{
    public interface IBuscarPorIdHandler
    {
        Task<Contracts.Response<ProdutoDTO?>> Handle(string id);
    }
}
