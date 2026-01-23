using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Categorie.Output
{
    [ExcludeFromCodeCoverage]
    public record CategorieOutputDto(Guid Id, string Name, bool IsEditavel);
}
