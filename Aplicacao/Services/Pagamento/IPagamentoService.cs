using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Services.Pagamento
{
    public interface IPagamentoService
    {
        Task<string> PagamentoQRCodeFakeAsync(decimal valor);

    }
}
