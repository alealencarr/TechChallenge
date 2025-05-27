using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Produtos.Buscar
{
    public class BuscarCommand
    {        
        public Domain.Entidades.Categoria? Categoria { get; set; }

    }
}
