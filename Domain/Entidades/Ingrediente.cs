using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class Ingrediente
    {
        public Ingrediente(string nome)
        {
            Nome = nome;
            Id = Guid.NewGuid();
        }
        public Guid Id { get; }
        public string Nome { get; set; }
    }
}
