using Shared.DTO.Authentication.Input;

namespace Shared.DTO.User.Output
{
    public record UserOutputDto(Guid Id, string Name, string Mail, List<RoleOutputDto> Roles);
}