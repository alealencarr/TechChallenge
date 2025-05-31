using Contracts.DTO.Produto;
using Domain.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Request.Produto
{
    public class AlterarRequest
    {
        public AlterarRequest(string nome, decimal preco, Guid categoriaId, List<ProdutoImagemRequest>? imagens, string descricao, List<IngredienteRequest>? ingredientes = null)
        {
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Imagens = imagens;
            CategoriaId = categoriaId;
            Ingredientes = ingredientes;
        }
        [Required(ErrorMessage = "É necessário informar o nome.")]
        [MaxLength(255)]

        public required string Nome { get; set; }

        [Required(ErrorMessage = "É necessário informar o preço.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "É necessário informar a categoria.")]
        public Guid CategoriaId { get; set; }

        public List<ProdutoImagemRequest>? Imagens { get; set; }

        [Required(ErrorMessage = "É necessário informar a descrição.")]
        [MaxLength(255)]

        public required string Descricao { get; set; }

        public List<IngredienteRequest>? Ingredientes { get; set; }
    }
}
