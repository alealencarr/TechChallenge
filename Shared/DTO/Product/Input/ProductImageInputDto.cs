namespace Shared.DTO.Product.Input;

public record ProductImageInputDto(Guid Id, string FileName, string MimeType, string ImagePath, string Name, byte[] Blob, Guid ProductId);