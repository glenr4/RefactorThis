using RefactorThis.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace RefactorThis.Domain.Interfaces
{
    public interface IProductRepository
    {
        public Task<Product> GetProduct(Guid id);
    }
}