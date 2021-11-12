﻿using RefactorThis.Domain.Entities;
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
                new Product(new Guid("4E2BC5F2-699A-4C42-802E-CE4B4D2AC000"),"Product1", "Description", 1,1),
                new Product(new Guid("4E2BC5F2-699A-4C42-802E-CE4B4D2AC001"),"Product2", "Description", 2,2),
                new Product(new Guid("4E2BC5F2-699A-4C42-802E-CE4B4D2AC002"),"Product3", "Description", 3,3),
            };
        }
    }
}