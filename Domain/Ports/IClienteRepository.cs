using Domain.Entidades;

namespace Domain.Ports
{
    public interface IClienteRepository
    {
        Task Adicionar(Cliente cliente);
        Task Alterar(Cliente cliente);

        Task<Cliente?> GetClientePorId(string id);
        Task<Cliente?> GetClientePorCPF(string CPF);
    }
}
