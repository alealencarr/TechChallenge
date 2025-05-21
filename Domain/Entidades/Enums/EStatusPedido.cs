using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.Enums
{
    public enum EStatusPedido
    {
        EmAberto = 1,
        Recebido = 2,
        EmPreparacao = 3,
        Pronto = 4,
        Finalizado = 5
    }
}
