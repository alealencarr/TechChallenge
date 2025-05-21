using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.ItemCardapio
{
    public abstract class Item
    {
        public Item(string nome, decimal preco, Categoria categoria, List<Imagem>? imagens, string descricao)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Imagens = imagens;
            Categoria = categoria;
        }
        public Guid Id { get; }
        public string Nome {  get; set; }

        public decimal Preco { get; set; }
        public Categoria Categoria { get; set; }

        public List<Imagem>? Imagens { get; set; }

        public string Descricao { get; set; }
    }
}
