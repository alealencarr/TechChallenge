using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Cliente.BuscarPorCPF
{
    public class BuscarPorCPFCommand
    {
        public string CPF { get; set; }

        public BuscarPorCPFCommand(string cpf)
        { 
            this.CPF = cpf; 
        }
    }
}
