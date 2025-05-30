using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Services
{
    public interface IFileSaver
    {
        Task SalvarArquivo(byte[] dados, string nomeArquivo, string subpasta);

    }
}
