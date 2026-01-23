using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class SignatureGenerator
    {
        public static string GenerateSignature(string jsonPayload, string secret)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(jsonPayload));
            return Convert.ToBase64String(hash);
        }
    }
}
