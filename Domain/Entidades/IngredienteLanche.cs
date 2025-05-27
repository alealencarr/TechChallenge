﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class IngredienteLanche
    {

        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();

        public Guid Id {  get; private set; }
        public Guid IdIngrediente { get; set; }
        public decimal Preco { get; set; } = 0M;
        public bool Adicional { get; set; } = false;
        public Guid ItemId { get; set; }
        public IngredienteLanche(Guid idIngrediente, bool adicional, decimal preco, Guid itemId)
        {
            Id = Guid.NewGuid();
            IdIngrediente = idIngrediente;
            Adicional = adicional;
            Preco = preco;
            ItemId = itemId;
        }
 
    }
}
