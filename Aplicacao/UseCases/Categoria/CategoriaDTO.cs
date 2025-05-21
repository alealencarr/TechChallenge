using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Categoria
{
    public class CategoriaDTO
    {
        public string Id { get; set; }

        public string Nome { get; set; }

        public CategoriaDTO() { }

        public CategoriaDTO(string id, string nome)
        {
            Nome = nome;
            Id = id;
        }
    }
}
