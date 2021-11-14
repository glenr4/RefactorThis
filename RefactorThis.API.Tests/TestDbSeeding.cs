using RefactorThis.Domain.Entities;
using RefactorThis.Persistence.Sqlite;
using System;
using System.Collections.Generic;

namespace RefactorThis.API.Tests
{
    public static class TestDbSeeding
    {
        private static IList<Guid> _productIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        private static IList<Guid> _productOptionIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        public static void Init(RefactorThisDbContext db)
        {
            db.Products.RemoveRange(db.Products);
            db.ProductOptions.RemoveRange(db.ProductOptions);

            db.Products.AddRange(GetSeedProducts());
            db.ProductOptions.AddRange(GetSeedProductOptions());
            db.SaveChanges();
        }

        public static List<Product> GetSeedProducts()
        {
            return new List<Product>()
            {
                new Product(_productIds[0],"Product1", "Description", 1,1),
                new Product(_productIds[1],"Product2", "Description", 2,2),
                new Product(_productIds[2],"Product3", "Description", 3,3),
            };
        }

        public static List<ProductOption> GetSeedProductOptions()
        {
            return new List<ProductOption>()
            {
                new ProductOption(_productOptionIds[0],_productIds[0],"ProductOption1", "Description"),
                new ProductOption(_productOptionIds[1],_productIds[1],"ProductOption2", "Description"),
                new ProductOption(_productOptionIds[2],_productIds[2],"ProductOption3", "Description"),
            };
        }
    }
}