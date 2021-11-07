using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Interfaces;
using RefactorThis.Persistence.Sqlite.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Persistence.Sqlite
{
    public class ProductRepository : IProductRepository
    {
        private readonly RefactorThisDbContext _context;

        public ProductRepository(RefactorThisDbContext context)
        {
            this._context = context;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<ProductOption> CreateProductOption(Guid productId, ProductOption productOption)
        {
            if (productId != productOption.ProductId) throw new ProductIdMismatchException(productId.ToString());

            // The SQlite database has not been confirgured with a many to one relationship between ProductOptions
            // and Product, so need to check that the Product exists here first.
            // TODO: either recreate the tables with the constraint or ensure it is done when upgrading to another database type.
            var product = await _context.Products.Where(p => p.Id == productId).FirstOrDefaultAsync();

            if (product == null) throw new DbUpdateException($"Product with Id: {productId} does not exist");

            _context.ProductOptions.Add(productOption);

            await _context.SaveChangesAsync();

            return productOption;
        }

        public Task<Product> GetProduct(Guid id)
        {
            return _context.Products.Where(p => p.Id == id).Include(p => p.ProductOptions).FirstOrDefaultAsync();
        }
    }
}