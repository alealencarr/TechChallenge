using Contracts.DTO.Produto;

namespace Aplicacao.UseCases.Produto.Alterar
{
    public class AlterarPorIdCommand
    {
        public AlterarPorIdCommand(string nome, decimal preco, Guid categoriaId, List<ProdutoImagemDTO>? imagens, string descricao, string id, List<IngredienteCommand>? ingredientes = null)
        {
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Imagens = imagens;
            CategoriaId = categoriaId;
            Ingredientes = ingredientes;
            Id = id;
        }
        public string Nome { get; set; }
        public string Id { get; set; }

        public decimal Preco { get; set; }

        public Guid CategoriaId { get; set; }

        public List<ProdutoImagemDTO>? Imagens { get; set; }

        public string Descricao { get; set; }

        public List<IngredienteCommand>? Ingredientes { get; set; }
    }
}