using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Produto
{
    public class IngredienteCommand
    {
        public Guid Id { get; set; }

        public int Quantidade { get; set; }

        public IngredienteCommand ()
        {

        }
        
        public IngredienteCommand(Guid id, int quantidade)
        {
            Id = id;
            Quantidade = quantidade;
        }

    }
}
