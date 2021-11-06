using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Interfaces;
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

        public Task<Product> GetProduct(Guid id)
        {
            return _context.Products.Where(p => p.Id == id).Include(p => p.ProductOptions).FirstOrDefaultAsync();
        }
    }
}