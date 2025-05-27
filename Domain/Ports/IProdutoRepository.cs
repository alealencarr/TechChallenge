using Domain.Entidades;


namespace Domain.Ports
{
    public interface IProdutoRepository
    {
        Task Adicionar(Produto produto);
        Task Alterar(Produto produto);
        Task Remover(Produto produto);

        Task<List<Produto>> Buscar(string? id, string? name);

        Task<Produto?> GetById (string id);

    }
}
