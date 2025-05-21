using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Cliente.BuscarClientePorCPF
{
    public class BuscarClientePorCPFCommand
    {
        public string CPF { get; set; }

        public BuscarClientePorCPFCommand(string cpf)
        { 
            this.CPF = cpf; 
        }
    }
}
