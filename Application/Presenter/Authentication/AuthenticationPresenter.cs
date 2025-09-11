using Shared.DTO.Authentication.Output;
using Shared.Result;

namespace Application.Presenter.Authentication
{
    public class AuthenticationPresenter
    {
        private string _message;
        public AuthenticationPresenter(string? message = null) { _message = message ?? string.Empty; }
        public ICommandResult<TokenDto> TransformObject(string token, string refresh)
        {
            return CommandResult<TokenDto>.Success(new TokenDto(token, refresh), _message);
        }
        public ICommandResult<string> TransformString(string token )
        {
            return CommandResult<string>.Success(token, _message);
        }

        
        public ICommandResult<T> Error<T>(string message)
        {
            return CommandResult<T>.Fail(message);
        }
    }
}
