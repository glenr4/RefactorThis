using RefactorThis.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace RefactorThis.Persistence
{
    public interface IProductOptionRepository
    {
        Task<ProductOption> GetProductOptionAsync(Guid productId, Guid productOptionId);

        Task<ProductOption> CreateProductOptionAsync(ProductOption productOption);

        Task<ProductOption> UpdateProductOptionAsync(ProductOption productOption);

        Task<PagedList<ProductOption>> GetAllProductOptionsAsync(Guid productId, int page = 1, int pageSize = 10);

        Task<Guid> DeleteProductOptionAsync(Guid productId, Guid productOptionId);
    }
}