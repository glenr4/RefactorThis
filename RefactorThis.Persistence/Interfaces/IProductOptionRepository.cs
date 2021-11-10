using RefactorThis.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace RefactorThis.Persistence
{
    public interface IProductOptionRepository
    {
        Task<ProductOption> GetProductOptionAsync(Guid id);

        Task<ProductOption> CreateProductOptionAsync(ProductOption productOption);

        Task<ProductOption> UpdateProductOptionAsync(ProductOption productOption);

        Task<Guid> DeleteProductOptionAsync(Guid id);

        Task<PagedList<ProductOption>> GetAllProductOptionsAsync(int page = 1, int pageSize = 10);
    }
}