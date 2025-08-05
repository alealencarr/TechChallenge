﻿using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Ingredient.Request
{
    public record IngredientRequestDto
    {
        [Required(ErrorMessage = "É necessário informar o preço.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "É necessário informar o nome.")]
        [MaxLength(255)]
        public required string Name { get; set; }
    }
}
