using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Exceptions;
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

        public Task<PagedList<Product>> GetAllProductsAsync(int page = 1, int pageSize = 10, string name = null)
        {
            var query = _context.QueryAll<Product>();
            if (name != null)
            {
                query = query.Where(p => p.Name == name);
            }
            else
            {
                query = query.OrderBy(p => p.Name);
            }

            return PagedList<Product>.ToPagedListAsync(query, page, pageSize);
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            var result = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

            return result ?? throw new ProductNotFoundException(id.ToString());
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            // The SQlite database has not been confirgured with a Primary Key, so duplicates can be added unless they are
            // prevented in the code.
            // TODO: either recreate the tables with the constraint or ensure it is done when upgrading to another database type.
            var existingProduct = await _context.Products.Where(p => p.Id == product.Id).FirstOrDefaultAsync();

            if (existingProduct != null) throw new DbUpdateException($"Product with Id: {product.Id} already exists");

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public Task<Product> DeleteProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}