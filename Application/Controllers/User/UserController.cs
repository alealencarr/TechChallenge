using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Application.Presenter.Users;
using Application.Presenter.Users;
using Application.UseCases.Users;
using Application.UseCases.Users;
using Shared.DTO.User.Output;
using Shared.DTO.User.Request;
using Shared.Result;

namespace Application.Controllers.User
{
    public class UserController
    {
        private IUserDataSource _dataSource;
        private IPasswordService? _passwordService;
        public UserController(IUserDataSource dataSource,  IPasswordService? passwordService = null)
        {
            _dataSource = dataSource;
            _passwordService = passwordService;
        }

        public async Task<ICommandResult<UserOutputDto?>> CreateUser(UserRequestDto user)
        {
            UserPresenter userPresenter = new("Usuário cadastrado!");

            try
            {
                var userGateway = UserGateway.Create(_dataSource);

                var useCaseCreate = CreateUserUseCase.Create(userGateway, _passwordService);

                var userEntity = await useCaseCreate.Run(user);

                return userPresenter.TransformObject(userEntity);
            }
            catch (Exception ex)
            {
                return userPresenter.Error<UserOutputDto?>(ex.Message);
            }

        }

        public async Task<ICommandResult> DeleteUser(Guid id)
        {
            UserPresenter userPresenter = new("Usuário excluido!");

            try
            {
                var userGateway = UserGateway.Create(_dataSource);

                var useCaseCreate = DeleteUserUseCase.Create(userGateway);

                await useCaseCreate.Run(id);

                return userPresenter.RetornoSucess();
            }
            catch (Exception ex)
            {
                return userPresenter.Error<ICommandResult>(ex.Message);
            }
        }
    }
}
