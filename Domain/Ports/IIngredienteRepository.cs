using Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports
{
    public interface IIngredienteRepository
    {
        Task<Ingrediente?> GetById(string Id);

        Task Adicionar(Ingrediente ingrediente);

        Task Alterar(Ingrediente ingrediente);

        Task<List<Ingrediente>?> GetAll();

    }
}
