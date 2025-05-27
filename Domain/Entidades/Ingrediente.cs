using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class Ingrediente
    {
        public Ingrediente(string nome, decimal preco)
        {
            Nome = nome;
            Id = Guid.NewGuid();
            Preco = preco;
        }
        public Guid Id { get; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
    }
}
