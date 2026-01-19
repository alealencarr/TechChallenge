using Domain.Entities;
using Shared.DTO.Categorie;

namespace Application.Interfaces.DataSources
{
    public interface ICategorieDataSource
    {
 
        Task<CategorieDto?> GetCategorieById(Guid id);
 

    }
}
