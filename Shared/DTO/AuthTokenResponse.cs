using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    [ExcludeFromCodeCoverage]
    public class AuthTokenResponse
    {
        public string access_token { get; set; } = "";
    }
}
