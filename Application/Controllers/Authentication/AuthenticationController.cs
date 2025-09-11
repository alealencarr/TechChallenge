using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.Interfaces.Services;
using Application.Presenter.Authentication;
using Application.UseCases.Authentication;
using Shared.DTO.Authentication.Output;
using Shared.DTO.Authentication.Request;
using Shared.Result;
using System.Linq;

namespace Application.Controllers.Authentication
{
    public class AuthenticationController
    {
        private IUserDataSource _dataSource;
        private ICustomerDataSource _dataSourceCustomer;
        private ITokenService _tokenService;
        private IPasswordService? _passwordService;
        public AuthenticationController(IUserDataSource dataSource, ITokenService tokenService, IPasswordService? passwordService)
        {
            _dataSource = dataSource;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        public AuthenticationController(ICustomerDataSource dataSource, ITokenService tokenService)
        {
            _dataSourceCustomer = dataSource;
            _tokenService = tokenService;            
        }

        public async Task<ICommandResult<TokenDto?>> Authentication(AuthenticationLoginRequestDto loginDto)
        {
            AuthenticationPresenter userPresenter = new("Token gerado!");

            try
            {
                var userGateway = UserGateway.Create(_dataSource);                
                var useCaseLogin = AuthenticationTokenUseCase.Create(userGateway, _passwordService, _tokenService);
                var userEntity = await useCaseLogin.Run(loginDto.ClientId, loginDto.ClientSecret);

                return userPresenter.TransformObject(userEntity.Token, userEntity.RefreshToken);
            }
            catch (Exception ex)
            {
                return userPresenter.Error<TokenDto?>(ex.Message);
            }
        }

        public async Task<ICommandResult<string?>> AuthenticationCustomer(string? cpf)
        {
            AuthenticationPresenter userPresenter = new("Token gerado!");

            try
            {
                var userGateway = CustomerGateway.Create(_dataSourceCustomer);
                var useCaseLogin = AuthenticationTokenCustomerUseCase.Create(userGateway,  _tokenService);
                var userEntity = await useCaseLogin.Run(cpf);

                return userPresenter.TransformString(userEntity);  
            }
            catch (Exception ex)
            {
                return userPresenter.Error<string?>(ex.Message);
            }
        }

        public async Task<ICommandResult<TokenDto?>> Refresh(string refreshToken)
        {
            AuthenticationPresenter userPresenter = new("Token gerado novamente!");

            try
            {
                var userGateway = UserGateway.Create(_dataSource);
                var useCaseLogin = AuthenticationRefreshTokenUseCase.Create(userGateway,  _tokenService);
                var userEntity = await useCaseLogin.Run(refreshToken);

                return userPresenter.TransformObject(userEntity.Token, userEntity.RefreshToken);
            }
            catch (Exception ex)
            {
                return userPresenter.Error<TokenDto?>(ex.Message);
            }
        }        
    }
}
