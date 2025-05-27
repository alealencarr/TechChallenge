using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class Imagem
    {
        public Guid Id { get; set; }
        
        public byte[] Blob { get; set; }

        public Guid ProdutoId { get; set; }

        public Produto Produto { get; set; }
    }
}
