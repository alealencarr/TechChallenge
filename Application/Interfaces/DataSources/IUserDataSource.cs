using Shared.DTO.Authentication.Input;

namespace Application.Interfaces.DataSources
{
    public interface IUserDataSource
    {
        Task<UserInputDto?> GetUserByMail(string mail);

        Task<UserInputDto?> GetUserByRefreshToken(string refresh);
        Task Create(UserInputDto user);

        Task<UserInputDto?> GetUserById(Guid id);

        Task Update(UserInputDto user);

        Task Delete(Guid id);
    }
}
