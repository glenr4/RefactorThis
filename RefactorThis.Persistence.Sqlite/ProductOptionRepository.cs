using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Persistence.Sqlite
{
    public class ProductOptionRepository : IProductOptionRepository
    {
        private readonly RefactorThisDbContext _context;

        public ProductOptionRepository(RefactorThisDbContext context)
        {
            this._context = context;
        }

        public async Task<ProductOption> CreateProductOptionAsync(ProductOption productOption)
        {
            // The SQlite database has not been confirgured with a many to one relationship between ProductOptions
            // and Product, so need to check that the Product exists here first.
            // TODO: either recreate the tables with the constraint or ensure it is done when upgrading to another database type.
            var product = await _context.Products.Where(p => p.Id == productOption.ProductId).FirstOrDefaultAsync();

            if (product == null) throw new DbUpdateException($"Product with Id: {productOption.ProductId} does not exist");

            _context.ProductOptions.Add(productOption);

            await _context.SaveChangesAsync();

            return productOption;
        }

        public Task<ProductOption> DeleteProductOptionAsync(ProductOption productOption)
        {
            throw new NotImplementedException();
        }

        public Task<ProductOption> GetProductOptionAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductOption> UpdateProductOptionAsync(ProductOption product)
        {
            throw new NotImplementedException();
        }
    }
}