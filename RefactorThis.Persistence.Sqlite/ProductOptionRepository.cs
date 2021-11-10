using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Exceptions;
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

        public Task<PagedList<ProductOption>> GetAllProductOptionsAsync(int page = 1, int pageSize = 10)
        {
            var query = _context.QueryAll<ProductOption>().OrderBy(p => p.Name);

            return PagedList<ProductOption>.ToPagedListAsync(query, page, pageSize);
        }

        public async Task<ProductOption> GetProductOptionAsync(Guid id)
        {
            var result = await _context.ProductOptions.Where(p => p.Id == id).FirstOrDefaultAsync();

            return result ?? throw new ProductOptionNotFoundException(id.ToString());
        }

        public async Task<ProductOption> CreateProductOptionAsync(ProductOption productOption)
        {
            // The SQlite database has not been confirgured with a many to one relationship between ProductOptions
            // and Product, so need to check that the Product exists here first.
            // TODO: either recreate the tables with the constraint or ensure it is done when upgrading to another database type.
            var existingProduct = await _context.Products.Where(p => p.Id == productOption.ProductId).FirstOrDefaultAsync();

            if (existingProduct == null) throw new DbUpdateException($"Product with Id: {productOption.ProductId} does not exist");

            // The SQlite database has not been confirgured with a Primary Key, so duplicates can be added unless they are
            // prevented in the code.
            // TODO: either recreate the tables with the constraint or ensure it is done when upgrading to another database type.
            var existingProductOption = await _context.ProductOptions.Where(p => p.Id == productOption.Id).FirstOrDefaultAsync();

            if (existingProductOption != null) throw new DbUpdateException($"ProductOption with Id: {existingProductOption.Id} already exists");

            _context.ProductOptions.Add(productOption);

            await _context.SaveChangesAsync();

            return productOption;
        }

        public async Task<ProductOption> UpdateProductOptionAsync(ProductOption ProductOption)
        {
            _context.ProductOptions.Update(ProductOption);

            await _context.SaveChangesAsync();

            return ProductOption;
        }

        public async Task<Guid> DeleteProductOptionAsync(Guid id)
        {
            var ProductOption = new ProductOption(id, Guid.NewGuid(), "Delete Me", "Delete Me");

            _context.ProductOptions.Remove(ProductOption);

            await _context.SaveChangesAsync();

            return id;
        }
    }
}