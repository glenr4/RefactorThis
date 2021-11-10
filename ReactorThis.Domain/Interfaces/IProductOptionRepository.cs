using RefactorThis.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace RefactorThis.Domain.Interfaces
{
    public interface IProductOptionRepository
    {
        Task<ProductOption> GetProductOptionAsync(Guid id);

        Task<ProductOption> CreateProductOptionAsync(ProductOption product);

        Task<ProductOption> UpdateProductOptionAsync(ProductOption product);

        Task<ProductOption> DeleteProductOptionAsync(ProductOption productOption);
    }
}