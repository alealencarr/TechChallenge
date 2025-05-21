using Domain.Entidades;
using Domain.Entidades.ItemCardapio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports
{
    public interface IProdutoRepository
    {
        Task Adicionar(Produto produto);
        Task Alterar(Produto produto);
        Task Remover(Produto produto);

        Task<List<Produto>> Buscar(Categoria? categoria);

        Task<Produto?> BuscarPorID (string id);

    }
}
