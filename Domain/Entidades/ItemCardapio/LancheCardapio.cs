using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.ItemCardapio
{
    public class LancheCardapio : Item
    {
        public LancheCardapio(string nome, decimal preco, Categoria categoria, List<Imagem>? imagens, string descricao, List<Ingrediente> ingredientes) : base(nome, preco, categoria, imagens, descricao)
        {
            Ingredientes = ingredientes;
        }

        public List<Ingrediente> Ingredientes { get; set; }
        
    }
}


