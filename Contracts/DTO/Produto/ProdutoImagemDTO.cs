using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO.Produto
{
    public class ProdutoImagemDTO
    {
        public string Url { get; set; }
        public string Nome { get; set; }
        
        //Comentado porque a resposta fica muito grande na visualização do Swagger
        public string Mimetype; //{ get; set; }
    }
}
