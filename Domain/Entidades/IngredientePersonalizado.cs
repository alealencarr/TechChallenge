using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class IngredientePersonalizado
    {
        public Guid Id { get; set; }
        public decimal Preco { get; set; } = 0M;
        public bool Adicional { get; set; } = false;
    }
}
