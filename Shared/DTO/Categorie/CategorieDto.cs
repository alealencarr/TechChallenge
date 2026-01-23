using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Categorie
{
    [ExcludeFromCodeCoverage]
    public record CategorieDto(Guid Id, string Name, bool IsEditavel, DateTime CreatedAt);
}
