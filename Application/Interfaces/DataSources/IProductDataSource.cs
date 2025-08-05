using Shared.DTO.Product.Input;

namespace Application.Interfaces.DataSources
{

    public interface IProductDataSource
    {
        Task Create(ProductInputDto customer);
        Task Update(ProductInputDto customer);
        Task Delete(Guid id);
        Task<ProductInputDto> GetById(Guid id);
        Task<List<ProductInputDto>> GetByCategorie(string? id, string? name);
        Task<List<ProductInputDto>> GetByIds(List<Guid> ids);

    }
}
