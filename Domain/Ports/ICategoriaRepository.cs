using Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports
{
    public interface ICategoriaRepository
    {
        Task<List<Categoria>> GetAll();

        Task<Categoria?> GetById(string id);
    }
}
