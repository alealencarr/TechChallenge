using Application.Gateways;
using Domain.Entities;
using Domain.ValueObjects;
using Shared.DTO.Categorie.Request;

namespace Application.UseCases.Categories
{

    public class UpdateCategorieUseCase
    {
        CategorieGateway _gateway = null;
        public static UpdateCategorieUseCase Create(CategorieGateway gateway)
        {
            return new UpdateCategorieUseCase(gateway);
        }

        private UpdateCategorieUseCase(CategorieGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Categorie> Run(CategorieRequestDto categorie, Guid id)
        {
            try
            {
                var categorieExists = await _gateway.GetById(id);

                if (categorieExists is null)
                    throw new Exception($"Error: Categorie not find by Id.");

                categorieExists.Name = categorie.Name;
                categorieExists.IsEditavel = categorie.IsEditavel;               

                await _gateway.UpdateCategorie(categorieExists);

                return (categorieExists);
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
}
