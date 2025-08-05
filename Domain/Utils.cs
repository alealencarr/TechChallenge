using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public static class Utils
    {
        public static string FormataCpfSemPontuacao(this string cpf)
        {

            return cpf.Replace(".","").Replace("-","");
        }

        public static string BaseUrl { get; private set; } = string.Empty;

        public static void Configure(string baseUrl)
        {
            BaseUrl = baseUrl?.TrimEnd('/') ?? throw new ArgumentNullException(nameof(baseUrl));
        }

        public static string ToAbsoluteUrl(this string relativePath)
        {
            if (string.IsNullOrWhiteSpace(BaseUrl))
                throw new InvalidOperationException("BaseUrl was not configured. Call ApplicationUrls.Configure(baseUrl) at startup.");

            return $"{BaseUrl}/{relativePath.TrimStart('/')}";
        }
    }
}
