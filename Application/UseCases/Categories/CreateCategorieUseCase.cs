using Application.Gateways;
using Domain.Entities;
using Shared.DTO.Categorie.Request;

namespace Application.UseCases.Categories;
public class CreateCategorieUseCase
{
    CategorieGateway _gateway = null;
    public static CreateCategorieUseCase Create(CategorieGateway gateway)
    {
        return new CreateCategorieUseCase(gateway);
    }

    private CreateCategorieUseCase(CategorieGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task<(Categorie, bool)> Run(CategorieRequestDto categorie)
    {
        try
        {
            var categorieExists = await _gateway.GetByName(categorie.Name);

            if (categorieExists is not null)
                return (categorieExists, true);

            var categorieEntity = new Categorie(categorie.Name, categorie.IsEditavel);

            await _gateway.CreateCategorie(categorieEntity);

            return (categorieEntity, false);
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error:{ex.Message}");
        }
    }
}
 