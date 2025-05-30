using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Common
{
    public static class Utils
    {
        public static string FormataCpfSemPontuacao(this string cpf)
        {

            return cpf.Replace(".","").Replace("-","");
        }
        
    }
}
