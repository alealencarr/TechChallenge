using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.ItemCardapio
{
    public class ItemComplementoCardapio : Item //Itens imutaveis -> Bebida, acompanhamento e sobremesa
    {
        public ItemComplementoCardapio(string nome, decimal preco, Categoria categoria, List<Imagem>? imagens, string descricao) : base(nome, preco, categoria, imagens, descricao)
        {
        }
    }
}
