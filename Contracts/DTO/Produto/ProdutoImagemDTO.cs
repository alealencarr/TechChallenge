using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO.Produto
{
    public class ProdutoImagemDTO
    {
        public string? Nome { get; set; }
        public byte[] Blob { get; set; } = default!;
    }
}
