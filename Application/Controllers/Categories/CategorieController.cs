using Application.Gateways;
using Application.Interfaces.DataSources;
using Application.Presenter.Categories;
using Application.Presenter.Products;
using Application.UseCases.Categories;
using Application.UseCases.Products;
using Shared.DTO.Categorie.Output;
using Shared.DTO.Categorie.Request;
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

        public async Task<ICommandResult> DeleteCategorie(Guid id)
        { 
            CategoriePresenter categoriePresenter = new("Categoria excluida!");

            try
            {
                var categorieGateway = CategorieGateway.Create(_dataSource);

                var useCaseCreate = DeleteCategorieUseCase.Create(categorieGateway);

                await useCaseCreate.Run(id);

                return categoriePresenter.RetornoSucess();
            }
            catch (Exception ex)
            {
                return categoriePresenter.Error<ICommandResult>(ex.Message);
            }
        }
        
        public async Task<ICommandResult<CategorieOutputDto>> UpdateCategorie(CategorieRequestDto categorieRequestDto, Guid id)
        {
            CategoriePresenter categoriePresenter = new("Categoria atualizada!");

            try
            {
                var categorieGateway = CategorieGateway.Create(_dataSource);
                var useCaseCreate = UpdateCategorieUseCase.Create(categorieGateway);
                var categorieEntity = await useCaseCreate.Run(categorieRequestDto, id);

                var dtoRetorno = categoriePresenter.TransformObject(categorieEntity);

                return dtoRetorno;
            }
            catch (Exception ex)
            {
                return categoriePresenter.Error<CategorieOutputDto>(ex.Message);
            }
        }

        public async Task<ICommandResult<CategorieOutputDto>> CreateCategorie(CategorieRequestDto categorieRequestDto)
        {
            CategoriePresenter categoriePresenter = new("Categoria criada!");

            try
            {
                var categorieGateway = CategorieGateway.Create(_dataSource);
                var useCaseCreate = CreateCategorieUseCase.Create(categorieGateway);
                var categorieEntity = await useCaseCreate.Run(categorieRequestDto);

                if (!categorieEntity.Item2)
                    return categoriePresenter.TransformObject(categorieEntity.Item1);

                return categoriePresenter.Conflict<CategorieOutputDto>($"Categoria com esse nome:{categorieRequestDto.Name} já existe.");
            }
            catch (Exception ex)
            {
                return categoriePresenter.Error<CategorieOutputDto>(ex.Message);
            }
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
