using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO.Pedido
{
    public class IngredienteItemPedidoDTO
    {

        public string IdIngrediente { get; set; }

        public int Quantidade { get; set; }

        public decimal Preco { get; set; }

        public bool Adicional { get; set; }
        public IngredienteItemPedidoDTO(string idIngrediente, int quantidade, decimal preco, bool adicional)
        {
            IdIngrediente = idIngrediente;
            Quantidade = quantidade;
            Preco = preco;
            Adicional = adicional;
        }
    }
}
