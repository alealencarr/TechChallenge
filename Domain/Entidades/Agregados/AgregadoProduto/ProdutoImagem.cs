using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.Agregados.AgregadoProduto
{
    public class ProdutoImagem
    {
        public Guid ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public string FileName { get; set; } 
        public string MimeType { get; set; }
        public string ImagePath { get; set; }
        public string Nome { get; set; }
        public byte[] Blob { get; set; } = default!;
        public Guid Id { get; set; }
        public ProdutoImagem(Guid produtoId, byte[] blob, string nome, string imagePath, string mimeType, string fileName)
        {
            Id = Guid.NewGuid();
            ProdutoId = produtoId;
            Blob = blob;
            Nome = nome;
            ImagePath = imagePath;
            MimeType = mimeType;
            FileName = fileName;
        }
        protected ProdutoImagem() { }
    }
}
