using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO.Pedido
{
    public class QRCodeDTO
    {
        public string Id { get; set; }
        public string Url { get; set; }

        public string Mimetype { get; set; }

        public QRCodeDTO(string id, string url, string mimetype)
        {
            Id = id;
            Url = url;
            Mimetype = mimetype;
        }


    }
}
