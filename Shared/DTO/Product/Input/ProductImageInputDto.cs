using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Product.Input;
[ExcludeFromCodeCoverage]
public record ProductImageInputDto(Guid Id, string FileName, string MimeType, string ImagePath, string Name, byte[] Blob, Guid ProductId);