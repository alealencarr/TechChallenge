﻿using System.ComponentModel.DataAnnotations;

namespace Contracts.Request.Produto
{
    public class IngredienteRequest
    {
        [Required(ErrorMessage = "Favor informar o Id do ingrediente.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Favor informar a Quantidade dos ingredientes que compõem os itens.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior ou igual a 1.")]

        public int Quantidade { get; set; }
    }
}
