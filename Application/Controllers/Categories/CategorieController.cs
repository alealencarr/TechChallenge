using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.Presenter.Categories;
using Application.UseCases.Categories;
using Shared.DTO.Categorie.Output;
using Shared.Result;

namespace Application.Controllers.Categories
{
    public class CategorieController
    {
        private ICategorieDataSource _dataSource;
        public CategorieController(ICategorieDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task<ICommandResult<List<CategorieOutputDto>>> GetAllCategoriesAsync()
        {
            CategoriePresenter categoriePresenter = new("Categorias encontradas!");

            try
            {
                var categorieGateway = CategorieGateway.Create(_dataSource);
                var useCase = GetAllCategoriesUseCase.Create(categorieGateway);
                var categoriesEntity = await useCase.Run();

                return categoriePresenter.TransformList(categoriesEntity);
            }
            catch (Exception ex)
            {
                return categoriePresenter.Error<List<CategorieOutputDto>>(ex.Message);
            }
        }

        public async Task<ICommandResult<CategorieOutputDto?>> GetCategorieByIdAsync(Guid id)
        {
            CategoriePresenter categoriePresenter = new("Categoria encontrada!");

            try
            {
                var categorieGateway = CategorieGateway.Create(_dataSource);
                var useCase = GetCategorieByIdUseCase.Create(categorieGateway);
                var categorie = await useCase.Run(id);

                return categorie is null ? categoriePresenter.Error<CategorieOutputDto?>("Categorie not found.") : categoriePresenter.TransformObject(categorie);
            }
            catch (Exception ex)
            {
                return categoriePresenter.Error<CategorieOutputDto?>(ex.Message);
            }
        }

    }
}
