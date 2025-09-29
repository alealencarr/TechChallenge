namespace Shared.DTO.Authentication.Input
{
    public record UserInputDto(Guid Id, string Name, string Mail, string Password, List<UserRolesInputDto> UserRoles, string? RefreshToken, DateTime? RefreshTokenExpiryTime);

}
