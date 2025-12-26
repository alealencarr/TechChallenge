using Shared.DTO.User.Output;
using Shared.DTO.User.Request;
using Shared.Result;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<CommandResult<UserOutputDto?>> Create(UserRequestDto userDto);
        Task<ICommandResult> Delete(Guid id);
    }
}
