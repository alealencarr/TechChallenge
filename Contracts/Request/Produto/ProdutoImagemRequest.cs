using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Request.Produto
{
    public class ProdutoImagemRequest
    {
        [Required(ErrorMessage="Favor informar o nome da imagem.")]
        public required string Nome { get; set; }
        public byte[] Blob { get; set; } = default!;
    }
}
