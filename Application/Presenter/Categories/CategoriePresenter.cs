using Domain.Entities;
using Shared.DTO.Categorie.Output;
using Shared.Result;

namespace Application.Presenter.Categories
{
    public class CategoriePresenter
    {
        private string _message;
        public CategoriePresenter(string? message = null) { _message = message ?? string.Empty; }
        public ICommandResult<CategorieOutputDto> TransformObject(Categorie categorie)
        {
            return CommandResult<CategorieOutputDto>.Success(Transform(categorie), _message);
        }

        public ICommandResult<List<CategorieOutputDto>> TransformList(List<Categorie> categories)
        {
            return CommandResult<List<CategorieOutputDto>>.Success(categories.Select(x => Transform(x)).ToList());
        }

        public CategorieOutputDto Transform(Categorie categorie)
        {
            return new CategorieOutputDto(categorie.Id , categorie.Name);
        }

        public ICommandResult<T> Error<T>(string message)
        {
            return CommandResult<T>.Fail(message);
        }

    }
}
