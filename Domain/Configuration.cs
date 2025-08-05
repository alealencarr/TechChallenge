using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class Configuration
    {
        public const HttpStatusCode HTTP_STATUS_CODE_DEFAULT = HttpStatusCode.OK;
        public const int DefaultPageSize = 25;
        public const int DefaultPageNumber = 1;
    }
}