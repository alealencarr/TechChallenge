using Domain.Entidades.ItemCardapio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Pedido.Criar
{
    public class CriarCommand
    {
        public CriarCommand(List<LancheCardapio>? lanches, List<ItemComplementoCardapio>? complementos)
        {
            Lanches = lanches;
            Complementos = complementos;
        }
        public List<LancheCardapio>? Lanches { get; }
        public List<ItemComplementoCardapio>? Complementos { get; set; }

     }
}
