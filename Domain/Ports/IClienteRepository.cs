using Domain.Entidades;

namespace Domain.Ports
{
    public interface IClienteRepository
    {
        Task Adicionar(Cliente cliente);
        Task Alterar(Cliente cliente);

        Task<Cliente?> GetById(string id);
        Task<Cliente?> GetByCPF(string CPF);
    }
}
