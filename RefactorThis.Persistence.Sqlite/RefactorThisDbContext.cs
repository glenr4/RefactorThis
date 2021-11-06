using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain.Entities;

namespace RefactorThis.Persistence.Sqlite
{
    public class RefactorThisDbContext : DbContext
    {
        public RefactorThisDbContext(DbContextOptions<RefactorThisDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
    }
}