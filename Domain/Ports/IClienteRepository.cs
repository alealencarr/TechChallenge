using Domain.Entidades;
using Domain.Entidades.ItemCardapio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports
{
    public interface IClienteRepository
    {
        Task Adicionar(Cliente cliente);
        Task Alterar(Cliente cliente);

        Task<Cliente?> GetClientePorCPF(string CPF);
    }
}
