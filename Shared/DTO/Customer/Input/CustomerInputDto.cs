namespace Shared.DTO.Categorie.Input;

public record CustomerInputDto(Guid Id, DateTime CreatedAt, string Cpf, string Name, string Mail, bool CustomerIdentified); 