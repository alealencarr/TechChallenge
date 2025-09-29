using Domain.Entities.Aggregates.AggregateUser;
using Shared.DTO.User.Output;
using Shared.Result;

namespace Application.Presenter.Users
{
    public class UserPresenter
    {
        private string _message;
        public UserPresenter(string? message = null) { _message = message ?? string.Empty; }
        public ICommandResult<UserOutputDto> TransformObject(User user)
        {
            return CommandResult<UserOutputDto>.Success(Transform(user), _message);
        }

        public ICommandResult<List<UserOutputDto>> TransformList(List<User> users)
        {
            return CommandResult<List<UserOutputDto>>.Success(users.Select(x => Transform(x)).ToList());
        }

        public UserOutputDto Transform(User user)
        {
            return new UserOutputDto(user.Id, user.Name, user.Mail, user.UserRoles.Select(x => new RoleOutputDto(x.Role.Name)).ToList());
        }

        public ICommandResult<T> Error<T>(string message)
        {
            return CommandResult<T>.Fail(message);
        }

        public ICommandResult<T> Conflict<T>(string message)
        {
            return CommandResult<T>.ConflictReturn(message);
        }

        public ICommandResult RetornoSucess()
        {
            return CommandResult.Success(_message);
        }

    }
}
