using RefactorThis.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace RefactorThis.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(Guid id);

        Task<Product> CreateProduct(Product product);
        Task<ProductOption> CreateProductOption(Guid productId, ProductOption productOption);
    }
}