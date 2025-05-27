using Domain.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Produtos.Alterar
{
    public class AlterarCommand
    {
        public AlterarCommand(string nome, decimal preco, Domain.Entidades.Categoria categoria, List<Imagem>? imagens, string descricao, List<Domain.Entidades.Ingrediente>? ingredientes = null)
        {
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Imagens = imagens;
            Categoria = categoria;
            Ingredientes = ingredientes;
        }
        [Required(ErrorMessage = "É necessário informar o nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "É necessário informar o preço.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "É necessário informar a categoria.")]
        public Domain.Entidades.Categoria Categoria { get; set; }

        public List<Imagem>? Imagens { get; set; }

        [Required(ErrorMessage = "É necessário informar a descrição.")]
        public string Descricao { get; set; }

        // Só necessário se for "Lanche"
        public List<Domain.Entidades.Ingrediente>? Ingredientes { get; set; }
    }
}
