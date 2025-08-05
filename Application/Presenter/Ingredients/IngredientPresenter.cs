using Domain.Entities;
using Shared.DTO.Ingredient.Output;
using Shared.Result;

namespace Application.Presenter.Ingredients
{
    public class IngredientPresenter
    {
        private string _message;
        public IngredientPresenter(string? message = null) { _message = message ?? string.Empty; }
        public ICommandResult<IngredientOutputDto> TransformObject(Ingredient ingredient)
        {
            return CommandResult<IngredientOutputDto>.Success(Transform(ingredient), _message);
        }

        public ICommandResult<List<IngredientOutputDto>> TransformList(List<Ingredient> ingredients)
        {
            return CommandResult<List<IngredientOutputDto>>.Success(ingredients.Select(x => Transform(x)).ToList());
        }

        public IngredientOutputDto Transform(Ingredient ingredient)
        {
            return new IngredientOutputDto { Id = ingredient.Id, Name = ingredient.Name, Price = ingredient.Price };
        }

        public ICommandResult<T> Error<T>(string message)
        {
            return CommandResult<T>.Fail(message);
        }

    }
}
