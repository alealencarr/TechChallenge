using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Pedido.SharedCommand
{
    public class IngredienteCommand
    {
        public Guid Id { get; set; }

        public bool Adicional { get; set; } = false;

    }
}
