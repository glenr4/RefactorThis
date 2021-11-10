using RefactorThis.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace RefactorThis.Persistence
{
    public interface IProductOptionRepository
    {
        Task<ProductOption> GetProductOptionAsync(Guid id);

        Task<ProductOption> CreateProductOptionAsync(ProductOption product);

        Task<ProductOption> UpdateProductOptionAsync(ProductOption product);

        Task<ProductOption> DeleteProductOptionAsync(ProductOption productOption);
    }
}