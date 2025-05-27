using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Ingrediente.BuscarPorId
{
    public class BuscarPorIdCommand
    {

        public string Id { get; set; }

        public BuscarPorIdCommand(string id)
        { 
            this.Id = id; 
        }
    }
}
