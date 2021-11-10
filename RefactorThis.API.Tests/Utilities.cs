using RefactorThis.Domain.Entities;
using RefactorThis.Persistence.Sqlite;
using System;
using System.Collections.Generic;

namespace RefactorThis.API.Tests
{
    public static class Utilities
    {
        public static void InitializeDbForTests(RefactorThisDbContext db)
        {
            db.Products.AddRange(GetSeedingProducts());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(RefactorThisDbContext db)
        {
            db.Products.RemoveRange(db.Products);
            InitializeDbForTests(db);
        }

        public static List<Product> GetSeedingProducts()
        {
            return new List<Product>()
            {
                new Product(Guid.NewGuid(),"Product1", "Description", 1,1),
                new Product(Guid.NewGuid(),"Product2", "Description", 2,2),
                new Product(Guid.NewGuid(),"Product3", "Description", 3,3),
            };
        }
    }
}