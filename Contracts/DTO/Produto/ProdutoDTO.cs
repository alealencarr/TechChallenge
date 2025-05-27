using Domain.Entidades;
using System.Text.Json.Serialization;

namespace Contracts.DTO.Produto
{
    public class ProdutoDTO
    {
        public ProdutoDTO(string nome, decimal preco, Domain.Entidades.Categoria categoria, List<Imagem>? imagens, string descricao, string id, List<Domain.Entidades.Ingrediente>? ingredientes = null)
        {
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Imagens = imagens;
            Categoria = categoria;
            Ingredientes = ingredientes;
            Id = id;
        }
        public ProdutoDTO() { }

        public string Id { get; set; }
        public string Nome { get; set; }

        public decimal Preco { get; set; }

        public Domain.Entidades.Categoria Categoria { get; set; }

        public List<Imagem>? Imagens { get; set; }

        public string Descricao { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Domain.Entidades.Ingrediente>? Ingredientes { get; set; }
    }
}
