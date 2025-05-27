using Domain.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Request.Produto
{
    public class CriarRequest
    {
        public CriarRequest(string nome, decimal preco, Guid categoriaId, List<Imagem>? imagens, string descricao, List<Guid>? ingredientes = null)
        {
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Imagens = imagens;
            CategoriaId = categoriaId;
            Ingredientes = ingredientes;
        }
        [Required(ErrorMessage = "É necessário informar o nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "É necessário informar o preço.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "É necessário informar a categoria.")]
        public Guid CategoriaId { get; set; }

        public List<Imagem>? Imagens { get; set; }

        [Required(ErrorMessage = "É necessário informar a descrição.")]
        public string Descricao { get; set; }

        public List<Guid>? Ingredientes { get; set; }
    }
}
