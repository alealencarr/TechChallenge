using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class Categoria
    {
        public Categoria(string nome)
        {
            Nome = nome;
            Id = Guid.NewGuid();
        }
        public Guid Id { get; }
        public string Nome { get; set; } = string.Empty;
    }
}
