using Domain.Entidades;
using Domain.Entidades.ItemCardapio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports
{
    public interface IItemComplementoCardapioRepository
    {
        void Adicionar(ItemComplementoCardapio itemComplemento);
        void Alterar(ItemComplementoCardapio itemComplemento);
        void Remover(ItemComplementoCardapio itemComplemento);

        List<ItemComplementoCardapio> BuscarPorCategoria(Categoria categoria);
    }
}
