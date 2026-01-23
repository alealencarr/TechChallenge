using System.Diagnostics.CodeAnalysis;

namespace Shared.Configurations
{
    [ExcludeFromCodeCoverage]

    public class AuthClientOptions
    {
        public string ClientId { get; set; } = "";
        public string ClientSecret { get; set; } = "";
    }

}
