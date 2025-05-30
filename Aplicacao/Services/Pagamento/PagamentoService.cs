using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Services.Pagamento
{
    public class PagamentoService :  IPagamentoService
    {
        public async Task<string> PagamentoQRCodeFakeAsync(decimal valor)
        {
            var randomCode = $"qrcode-{Guid.NewGuid()}{valor}";
            return await Task.FromResult(randomCode);
        }
    }
}
