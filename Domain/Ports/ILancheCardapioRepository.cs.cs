using Domain.Entidades;
using Domain.Entidades.ItemCardapio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports
{
    public interface ILancheCardapioRepository
    {
        void Adicionar(LancheCardapio lanche);
        void Alterar(LancheCardapio lanche);
        void Remover(LancheCardapio lanche);

        List<LancheCardapio> BuscarPorCategoria(Categoria categoria);
    }
}
