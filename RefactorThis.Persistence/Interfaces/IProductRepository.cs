using RefactorThis.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace RefactorThis.Persistence
{
    public interface IProductRepository
    {
        Task<Product> GetProductAsync(Guid id);

        Task<Product> CreateProductAsync(Product product);

        Task<Product> UpdateProductAsync(Product product);

        Task<PagedList<Product>> GetAllProductsAsync(int page = 1, int pageSize = 10, string name = null);

        Task<Guid> DeleteProductAsync(Guid id);
    }
}