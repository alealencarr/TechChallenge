 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Aplicacao.Common
{
    public class Response<TData>
    {

        private readonly HttpStatusCode _code;

        [JsonConstructor]
        public Response()
             =>
            _code = Configuration.HTTP_STATUS_CODE_DEFAULT;

        public Response(TData? data, HttpStatusCode code = Configuration.HTTP_STATUS_CODE_DEFAULT, string? message = null)
        {
            _code = code;
            Data = data;
            Message = message;
        }

        public TData? Data { get; set; }
        public string? Message { get; set; }

        [JsonIgnore]
        public bool IsSucess => (int)_code is >= (int)HttpStatusCode.OK and < (int)HttpStatusCode.MultipleChoices;

    }
}