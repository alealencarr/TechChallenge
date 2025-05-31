using Contracts.DTO.Categoria;
using Contracts.DTO.Ingrediente;
using Contracts.Request.Produto;
using Domain.Entidades;
using System.Text.Json.Serialization;

namespace Contracts.DTO.Produto
{
    public class ProdutoDTO
    {
        public ProdutoDTO(string nome, decimal preco, CategoriaDTO? categoria, List<ProdutoImagemDTO> imagens, string descricao, string id, List<ProdutoIngredienteDTO> ingredientes)
        {
            Id = id;
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Categoria = categoria;

            Imagens = imagens;
            Ingredientes = ingredientes;
        }
        public ProdutoDTO() { }

        public string Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }

        public CategoriaDTO? Categoria { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ProdutoIngredienteDTO> Ingredientes { get; set; } = [];

        public List<ProdutoImagemDTO> Imagens { get; set; } = [];

    }
}
