using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class IngredienteLanche
    {
        public Guid IdIngrediente { get; set; }
        public decimal Preco { get; set; } = 0M;
        public bool Adicional { get; set; } = false;
        public Guid ItemId { get; set; }
        public IngredienteLanche(Guid id, bool adicional, decimal preco, Guid itemId)
        {
            IdIngrediente = id;
            Adicional = adicional;
            Preco = preco;
            ItemId = itemId;
        }
 
    }
}
