using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain.Entities;
using System.Linq;

namespace RefactorThis.Persistence.Sqlite
{
    public class RefactorThisDbContext : DbContext
    {
        public RefactorThisDbContext(DbContextOptions<RefactorThisDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }

        /// <summary>
        /// All of the entities in the DbSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> QueryAll<T>() where T : class
        {
            return this.Set<T>();
        }
    }
}