using Shared.DTO.Categorie.Output;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Shared.DTO.Product.Output
{
    [ExcludeFromCodeCoverage]
    public record ProductImageOutputDto(string Url, string Name);
     
} 